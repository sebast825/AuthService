using Aplication.Services;
using Azure.Core;
using Core.Constants;
using Core.Dto.Auth;
using Core.Dto.User;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Data;
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
        private readonly IEmailAttemptsService _EmailAttemptsService;
        private readonly IUserLoginHistoryService _loginAttemptsService;
        private readonly ISecurityLoginAttemptService _securityLoginAttemptService;
        private readonly IUnitOfWork _unitOfWork;
        public AuthUseCase(IUserServices userServices, IJwtService jwtService, IRefreshTokenService refreshTokenService,
            IEmailAttemptsService EmailAttemptsService, IUserLoginHistoryService loginAttemptsService,
            ISecurityLoginAttemptService securityLoginAttemptService,
            IUnitOfWork unitOfWork)
        {
            _userServices = userServices;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _EmailAttemptsService = EmailAttemptsService;
            _loginAttemptsService = loginAttemptsService;
            _securityLoginAttemptService = securityLoginAttemptService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto, string ipAddress, string deviceInfo)
        {
            await ThrowAndRegisterIfEmailIsBlockAsync(loginDto.Email, ipAddress, deviceInfo);

            try
            {
                UserResponseDto userResponseDto = await _userServices.ValidateCredentialsAsync(loginDto);

                return await HandleSuccessfulLoginAsync(userResponseDto, loginDto.Email, ipAddress, deviceInfo);
            }
            catch (InvalidCredentialException ex)
            {
                await RegisterFailedLoginAndThrowAsync(loginDto.Email, ipAddress, deviceInfo);
                throw;
            }


        }

        private async Task ThrowAndRegisterIfEmailIsBlockAsync(string email, string ipAddress, string deviceInfo)
        {
            bool emailIsBlocked = _EmailAttemptsService.EmailIsBlocked(email);
            if (emailIsBlocked)
            {
                await _securityLoginAttemptService.AddFailedLoginAttemptAsync(email, LoginFailureReasons.TooManyAttempts, ipAddress, deviceInfo);
                throw new InvalidOperationException(ErrorMessages.MaxLoginAttemptsExceeded);

            }
        }

        /* Record email attempt in DB and reset in cache.
         * Call fn to generate tokens */
        private async Task<AuthResponseDto> HandleSuccessfulLoginAsync(UserResponseDto userResponseDto, string email, string ipAddress, string deviceInfo)
        {
            _EmailAttemptsService.ResetAttempts(email);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _loginAttemptsService.AddSuccessAttemptAsync(userResponseDto.Id, ipAddress, deviceInfo);
                AuthResponseDto authResponseDto = await HandleTokenAsync(userResponseDto);
                await _unitOfWork.CommitAsync();
                return authResponseDto;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private async Task<AuthResponseDto> HandleTokenAsync(UserResponseDto userResponseDto)
        {
            string jwtToken = _jwtService.GenerateAccessToken(userResponseDto.Id.ToString());
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
            _EmailAttemptsService.IncrementAttempts(email);
            await _securityLoginAttemptService.AddFailedLoginAttemptAsync(email, LoginFailureReasons.InvalidCredentials, ipAddress, deviceInfo);
            throw new InvalidOperationException(ErrorMessages.InvalidCredentials);
        }


    }
}
