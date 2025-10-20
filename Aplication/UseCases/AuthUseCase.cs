using Aplication.Services;
using Core.Constants;
using Core.Dto.Auth;
using Core.Dto.User;
using Core.Entities;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

            bool emailIsBlocked = _EmailAttemptsServiceI.EmailIsBlocked(loginDto.Email);
            if (emailIsBlocked)
            {
                await _securityLoginAttemptServiceI.AddFailedLoginAttemptAsync(loginDto.Email, LoginFailureReasons.TooManyAttempts, ipAddress, deviceInfo);
                throw new InvalidOperationException(ErrorMessages.MaxLoginAttemptsExceeded);

            }
            try
            {
                UserResponseDto userResponseDto = await _userServicesI.ValidateCredentialsAsync(loginDto);
                //await HandleAttemptLogin(loginDto.Email, userResponseDto.Id, "updateIp");
                _EmailAttemptsServiceI.ResetAttempts(loginDto.Email);
                
                await _loginAttemptsServiceI.AddSuccessAttemptAsync(userResponseDto.Id, ipAddress,deviceInfo);


                string jwtToken = _jwtServiceI.GenerateAccessToken(userResponseDto.Id.ToString());
                RefreshToken refreshToken = _refreshTokenServiceI.CreateRefreshToken(userResponseDto.Id);
                await _refreshTokenServiceI.AddAsync(refreshToken);
                AuthResponseDto authResponseDto = new AuthResponseDto()
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken.Token,
                    User = userResponseDto
                };
                return authResponseDto;


            }
            catch  (InvalidCredentialException ex){
                _EmailAttemptsServiceI.IncrementAttempts(loginDto.Email);
                await _securityLoginAttemptServiceI.AddFailedLoginAttemptAsync(loginDto.Email, LoginFailureReasons.InvalidCredentials, ipAddress, deviceInfo);

                //add register
                bool nowBlocked = _EmailAttemptsServiceI.EmailIsBlocked(loginDto.Email);
                if (nowBlocked)
                {
                    throw new InvalidOperationException(ErrorMessages.MaxLoginAttemptsExceeded);
                }

                throw new InvalidOperationException(ErrorMessages.InvalidCredentials);
            }
            catch (Exception ex) {
                throw;
            }
           
        }
       

    }
}
