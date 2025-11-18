using Aplication.Helpers;
using Aplication.Services;
using Azure.Core;
using Core.Constants;
using Core.Dto.Auth;
using Core.Dto.RefreshToken;
using Core.Dto.User;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
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
        private readonly IUserServices _userServices;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IEmailAttemptsService _emailAttemptsService;
        private readonly IUserLoginHistoryService _loginAttemptsService;
        private readonly ISecurityLoginAttemptService _securityLoginAttemptService;
        private readonly ILogger<AuthUseCase> _logger;
        public AuthUseCase(IUserServices userServices, IJwtService jwtService, IRefreshTokenService refreshTokenService,
            IEmailAttemptsService EmailAttemptsService, IUserLoginHistoryService loginAttemptsService,
            ISecurityLoginAttemptService securityLoginAttemptService,
            ILogger<AuthUseCase> logger)
        {
            _userServices = userServices;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _emailAttemptsService = EmailAttemptsService;
            _loginAttemptsService = loginAttemptsService;
            _securityLoginAttemptService = securityLoginAttemptService;
            _logger = logger;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto, string ipAddress, string deviceInfo)
        {
            LoginAttemptContext loginAttemptContext = new LoginAttemptContext(loginDto.Email, ipAddress, deviceInfo);
            await ThrowAndRegisterIfEmailIsBlockedAsync(loginAttemptContext);

            try
            {
                UserResponseDto userResponseDto = await _userServices.ValidateCredentialsAsync(loginDto);
                return await HandleSuccessfulLoginAsync(userResponseDto, loginAttemptContext);
            }
            catch (InvalidCredentialException ex)
            {
                await RegisterFailedLoginAndThrowAsync(loginAttemptContext);
                throw;
            }


        }
        public async Task<string> GenerateNewAccessTokenAsync(string refreshToken)
        {

            RefreshTokenResponseDto refreshTokenResponse = await _refreshTokenService.GetValidRefreshTokenAsync(refreshToken);

            string accessToken = _jwtService.GenerateAccessToken(refreshTokenResponse.UserId.ToString());
            return accessToken;

        }

        private async Task ThrowAndRegisterIfEmailIsBlockedAsync(LoginAttemptContext loginAttemptContext)
        {
            bool emailIsBlocked = _emailAttemptsService.EmailIsBlocked(loginAttemptContext.Email);
            if (emailIsBlocked)
            {
                try
                {
                    SecurityLoginAttempt securityAttempt = LoginEventMapper.SecurityLoginAttemptMapper(loginAttemptContext.Email, LoginFailureReasons.TooManyAttempts,
                        loginAttemptContext.IpAddress, loginAttemptContext.DeviceInfo);

                    await _securityLoginAttemptService.AddFailedLoginAttemptAsync(securityAttempt);

                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        "Blocked login attempt detected for {Email} from {IpAddress} ({DeviceInfo})",
                      loginAttemptContext.Email, loginAttemptContext.IpAddress, loginAttemptContext.DeviceInfo);
                }
                throw new InvalidOperationException(ErrorMessages.MaxLoginAttemptsExceeded);

            }
        }

        /* Record email attempt in DB and reset in cache.
         * Call fn to generate tokens */
        private async Task<AuthResponseDto> HandleSuccessfulLoginAsync(UserResponseDto userResponseDto, LoginAttemptContext loginAttemptContext)
        {

            _emailAttemptsService.ResetAttempts(loginAttemptContext.Email);
            UserLoginHistory userLoginHistory = LoginEventMapper.LoginHistoryMapper(userResponseDto.Id, loginAttemptContext.IpAddress, loginAttemptContext.DeviceInfo);
            await TryAddSuccessAttemptAsync(userLoginHistory);
            AuthResponseDto authResponseDto = await HandleTokenAsync(userResponseDto);

            return authResponseDto;
        }
        private async Task TryAddSuccessAttemptAsync(UserLoginHistory userLoginHistory)

        {
            try
            {
                await _loginAttemptsService.AddSuccessAttemptAsync(userLoginHistory);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to audit login success for user {UserId}", userLoginHistory.UserId);

            }
        }
        private async Task<AuthResponseDto> HandleTokenAsync(UserResponseDto userResponseDto)
        {
            string jwtToken = _jwtService.GenerateAccessToken(userResponseDto.Id.ToString());
            await _refreshTokenService.RevokeRefreshTokenIfExistAsync(userResponseDto.Id);
            RefreshToken refreshToken = _refreshTokenService.CreateRefreshToken(userResponseDto.Id);
            await _refreshTokenService.AddAsync(refreshToken);

            return new AuthResponseDto()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token,
                User = userResponseDto
            };
        }

        private async Task RegisterFailedLoginAndThrowAsync(LoginAttemptContext loginAttemptContext)
        {
            _emailAttemptsService.IncrementAttempts(loginAttemptContext.Email);
            SecurityLoginAttempt securityAttempt = LoginEventMapper.SecurityLoginAttemptMapper(loginAttemptContext.Email, LoginFailureReasons.InvalidCredentials,
                loginAttemptContext.IpAddress, loginAttemptContext.DeviceInfo);
            await _securityLoginAttemptService.AddFailedLoginAttemptAsync(securityAttempt);

            throw new InvalidOperationException(ErrorMessages.InvalidCredentials);
        }


    }
}
