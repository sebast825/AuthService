using Aplication.Services;
using Core.Constants;
using Core.Dto.Auth;
using Core.Dto.User;
using Core.Entities;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class AuthUseCase
    {
        private readonly UserServicesI _userServicesI;
        private readonly JwtServiceI _jwtServiceI;
        private readonly RefreshTokenServiceI _refreshTokenServiceI;
        private readonly EmailAttemptsServiceI _EmailAttemptsServiceI;
        private readonly UserLoginHistoryServiceI _loginAttemptsServiceI;
        private readonly SecurityLoginAttemptServiceI _securityLoginAttemptServiceI;
        public AuthUseCase(UserServicesI userServicesI, JwtServiceI jwtServiceI, RefreshTokenServiceI refreshTokenServiceI,
            EmailAttemptsServiceI EmailAttemptsServiceI, UserLoginHistoryServiceI loginAttemptsServiceI,
            SecurityLoginAttemptServiceI securityLoginAttemptServiceI)
        {
            _userServicesI = userServicesI;
            _jwtServiceI = jwtServiceI;
            _refreshTokenServiceI = refreshTokenServiceI;
            _EmailAttemptsServiceI = EmailAttemptsServiceI;
            _loginAttemptsServiceI = loginAttemptsServiceI;
            _securityLoginAttemptServiceI = securityLoginAttemptServiceI;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto, string ipAddress, string deviceInfo)
        {
            await ThrowIfEmailIsBlockAsync(loginDto.Email, ipAddress, deviceInfo);

            try
            {
                UserResponseDto userResponseDto = await _userServicesI.ValidateCredentialsAsync(loginDto);                
                _EmailAttemptsServiceI.ResetAttempts(loginDto.Email);
                await _loginAttemptsServiceI.AddSuccessAttemptAsync(userResponseDto.Id, ipAddress, deviceInfo);

                return await HandleTokenAsync(userResponseDto);
            }
            catch (InvalidCredentialException ex)
            {
                await HandleFailderAttemptAsync(loginDto.Email, ipAddress, deviceInfo);
                throw;
            }


        }

        private async Task ThrowIfEmailIsBlockAsync(string email, string ipAddress, string deviceInfo)
        {
            bool emailIsBlocked = _EmailAttemptsServiceI.EmailIsBlocked(email);
            if (emailIsBlocked)
            {
                await _securityLoginAttemptServiceI.AddFailedLoginAttemptAsync(email, LoginFailureReasons.TooManyAttempts, ipAddress, deviceInfo);
                throw new InvalidOperationException(ErrorMessages.MaxLoginAttemptsExceeded);

            }
        }
        private async Task<AuthResponseDto> HandleTokenAsync(UserResponseDto userResponseDto)
        {
            string jwtToken = _jwtServiceI.GenerateAccessToken(userResponseDto.Id.ToString());
            RefreshToken refreshToken = _refreshTokenServiceI.CreateRefreshToken(userResponseDto.Id);
            await _refreshTokenServiceI.AddAsync(refreshToken);
            return new AuthResponseDto()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token,
                User = userResponseDto
            };
            
        }
        private async Task HandleFailderAttemptAsync(string email, string ipAddress, string deviceInfo)
        {
            _EmailAttemptsServiceI.IncrementAttempts(email);
            await _securityLoginAttemptServiceI.AddFailedLoginAttemptAsync(email, LoginFailureReasons.InvalidCredentials, ipAddress, deviceInfo);

            bool nowIsBlocked = _EmailAttemptsServiceI.EmailIsBlocked(email);
            if (nowIsBlocked)
            {
                throw new InvalidOperationException(ErrorMessages.MaxLoginAttemptsExceeded);
            }

            throw new InvalidOperationException(ErrorMessages.InvalidCredentials);
        }


    }
}
