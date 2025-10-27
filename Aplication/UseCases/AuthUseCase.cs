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
using Infrastructure.EventBus.RabbitMQ;
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
        private readonly RabbitMqEventProducer _workProducer;
        public AuthUseCase(IUserServices userServices, IJwtService jwtService, IRefreshTokenService refreshTokenService,
            IEmailAttemptsService EmailAttemptsService, IUserLoginHistoryService loginAttemptsService,
            ISecurityLoginAttemptService securityLoginAttemptService,
            ILogger<AuthUseCase> logger, RabbitMqEventProducer workProducer)
        {
            _userServices = userServices;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _emailAttemptsService = EmailAttemptsService;
            _loginAttemptsService = loginAttemptsService;
            _securityLoginAttemptService = securityLoginAttemptService;
            _logger = logger;
            _workProducer = workProducer;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto, string ipAddress, string deviceInfo)
        {
            await ThrowAndRegisterIfEmailIsBlockedAsync(loginDto.Email, ipAddress, deviceInfo);

            try
            {
                UserResponseDto userResponseDto = await _userServices.ValidateCredentialsAsync(loginDto);
                return await HandleSuccessfulLoginAsync(userResponseDto, loginDto.Email, ipAddress, deviceInfo);
            }
            catch (InvalidCredentialException ex)
            {
                await _workProducer.PublicarLoginFallido("Failed Attempt");
                await RegisterFailedLoginAndThrowAsync(loginDto.Email, ipAddress, deviceInfo);
                throw;
            }


        }
        public async Task<string> GenerateNewAccessTokenAsync(string refreshToken)
        {

            RefreshTokenResponseDto refreshTokenResponse = await _refreshTokenService.GetValidRefreshTokenAsync(refreshToken);

            string accessToken = _jwtService.GenerateAccessToken(refreshTokenResponse.UserId.ToString());
            return accessToken;

        }

        private async Task ThrowAndRegisterIfEmailIsBlockedAsync(string email, string ipAddress, string deviceInfo)
        {
            bool emailIsBlocked = _emailAttemptsService.EmailIsBlocked(email);
            if (emailIsBlocked)
            {
                try
                {
                    await _securityLoginAttemptService.AddFailedLoginAttemptAsync(email, LoginFailureReasons.TooManyAttempts, ipAddress, deviceInfo);

                }
                catch(Exception ex)
                {
                    _logger.LogWarning(
                        "Blocked login attempt detected for {Email} from {IpAddress} ({DeviceInfo})",
                        email, ipAddress, deviceInfo);
                }
                throw new InvalidOperationException(ErrorMessages.MaxLoginAttemptsExceeded);

            }
        }

        /* Record email attempt in DB and reset in cache.
         * Call fn to generate tokens */
        private async Task<AuthResponseDto> HandleSuccessfulLoginAsync(UserResponseDto userResponseDto, string email, string ipAddress, string deviceInfo)
        {

            _emailAttemptsService.ResetAttempts(email);
            await TryAddSuccessAttemptAsync(userResponseDto.Id, ipAddress, deviceInfo);
            AuthResponseDto authResponseDto = await HandleTokenAsync(userResponseDto);

            return authResponseDto;
        }
        private async Task TryAddSuccessAttemptAsync(int userId, string ipAddress, string deviceInfo)

        {
            try
            {
                await _loginAttemptsService.AddSuccessAttemptAsync(userId, ipAddress, deviceInfo);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex,"Failed to audit login success for user {UserId}", userId);

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

        private async Task RegisterFailedLoginAndThrowAsync(string email, string ipAddress, string deviceInfo)
        {
            _emailAttemptsService.IncrementAttempts(email);
            await _securityLoginAttemptService.AddFailedLoginAttemptAsync(email, LoginFailureReasons.InvalidCredentials, ipAddress, deviceInfo);
            throw new InvalidOperationException(ErrorMessages.InvalidCredentials);
        }


    }
}
