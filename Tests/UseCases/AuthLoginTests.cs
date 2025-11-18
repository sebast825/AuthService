using Aplication.Helpers;
using Aplication.Services;
using Aplication.UseCases;
using Castle.Core.Logging;
using Core.Constants;
using Core.Dto.Auth;
using Core.Dto.RefreshToken;
using Core.Dto.User;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UseCases
{



    [TestClass]
    public class AuthLoginTests
    {
        private Mock<IUserServices> _mockUserServices;
        private Mock<IJwtService> _mockJwtService;
        private Mock<IRefreshTokenService> _mockRefreshTokenService;
        private Mock<IEmailAttemptsService> _mockEmailAttemptsService;
        private Mock<IUserLoginHistoryService> _mockLoginAttemptsService;
        private Mock<ISecurityLoginAttemptService> _mockSecurityLoginAttemptService;
        private Mock<IDbContextTransaction> _mockTransaction;
        private Mock<ILogger<AuthUseCase>> _mockLogger;
        private AuthUseCase _authUseCase;

        [TestInitialize]
        public void SetUp()
        {
            _mockUserServices = new Mock<IUserServices>();
            _mockJwtService = new Mock<IJwtService>();
            _mockRefreshTokenService = new Mock<IRefreshTokenService>();
            _mockEmailAttemptsService = new Mock<IEmailAttemptsService>();
            _mockLoginAttemptsService = new Mock<IUserLoginHistoryService>();
            _mockSecurityLoginAttemptService = new Mock<ISecurityLoginAttemptService>();
            _mockLogger = new Mock<ILogger<AuthUseCase>>();



            _authUseCase = new AuthUseCase(
                _mockUserServices.Object,
                _mockJwtService.Object,
                _mockRefreshTokenService.Object,
                _mockEmailAttemptsService.Object,
                _mockLoginAttemptsService.Object,
                _mockSecurityLoginAttemptService.Object,
                _mockLogger.Object
            );
        }


        [TestMethod]
        public async Task LoginAsync_WhenCredentialsAreValid_ShouldReturnTokens()
        {
            // Arrange
            int userId = 1;
            var loginDto = new LoginRequestDto { Email = "test@test.com", Password = "1234" };
            var userResponse = new UserResponseDto { Id = userId, FullName = "Carmelo Sanchez" };

            _mockEmailAttemptsService.Setup(s => s.EmailIsBlocked(loginDto.Email)).Returns(false);
            _mockUserServices.Setup(s => s.ValidateCredentialsAsync(loginDto))
                .ReturnsAsync(userResponse);
            _mockJwtService.Setup(s => s.GenerateAccessToken(userResponse.Id.ToString()))
                .Returns("jwt_token");
            _mockRefreshTokenService.Setup(s => s.CreateRefreshToken(userResponse.Id))
                .Returns(new RefreshToken { Token = "refresh_token" });
            UserLoginHistory loginHistory = LoginEventMapper.LoginHistoryMapper(userId, "127.0.0.1", "device");

            // Act
            var result = await _authUseCase.LoginAsync(loginDto, "127.0.0.1", "device");

            // Assert
            Assert.AreEqual("jwt_token", result.AccessToken);
            Assert.AreEqual("refresh_token", result.RefreshToken);
            Assert.AreEqual(userResponse, result.User);

            _mockEmailAttemptsService.Verify(s => s.ResetAttempts(loginDto.Email), Times.Once);
            _mockRefreshTokenService.Verify(s => s.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
            _mockRefreshTokenService.Verify(s => s.RevokeRefreshTokenIfExistAsync(userId), Times.Once);
            _mockLoginAttemptsService.Verify(
               s => s.AddSuccessAttemptAsync(
                   It.Is<UserLoginHistory>(a =>
                       a.UserId == loginHistory.UserId&&
                       a.IpAddress == loginHistory.IpAddress &&
                       a.DeviceInfo == loginHistory.DeviceInfo)
           ),
           Times.Once);

        }
        [TestMethod]
        public async Task LoginAsync_WhenCredentialsAreInvalid_ShouldThrow()
        {
            // Arrange
            var loginDto = new LoginRequestDto { Email = "test@test.com", Password = "1234" };

            _mockEmailAttemptsService.Setup(s => s.EmailIsBlocked(loginDto.Email)).Returns(false);
            _mockUserServices.Setup(s => s.ValidateCredentialsAsync(loginDto))
                .ThrowsAsync(new InvalidCredentialException(ErrorMessages.InvalidCredentials));
            SecurityLoginAttempt securityAttempt = LoginEventMapper.SecurityLoginAttemptMapper(loginDto.Email, LoginFailureReasons.InvalidCredentials, "127.0.0.1", "device");

            // Act
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _authUseCase.LoginAsync(loginDto, "127.0.0.1", "device"));

            // Assert
            Assert.AreEqual(ErrorMessages.InvalidCredentials, ex.Message);
            _mockEmailAttemptsService.Verify(s => s.IncrementAttempts(loginDto.Email), Times.Once);
            _mockSecurityLoginAttemptService.Verify(
                s => s.AddFailedLoginAttemptAsync(
                    It.Is<SecurityLoginAttempt>(a =>
                        a.Email == securityAttempt.Email &&
                        a.FailureReason == securityAttempt.FailureReason &&
                        a.IpAddress == securityAttempt.IpAddress &&
                        a.DeviceInfo == securityAttempt.DeviceInfo)
            ),
            Times.Once);
        }
        [TestMethod]
        public async Task LoginAsync_WhenEmailIsBlocked_ShouldThrow()
        {
            // Arrange
            var loginDto = new LoginRequestDto { Email = "test@test.com", Password = "1234" };
            var userResponse = new UserResponseDto { Id = 1, FullName = "Carmelo Sanchez" };
            _mockEmailAttemptsService.Setup(s => s.EmailIsBlocked(loginDto.Email)).Returns(true);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _authUseCase.LoginAsync(loginDto, "127.0.0.1", "device"));

            // Assert
            _mockEmailAttemptsService.Verify(s => s.EmailIsBlocked(loginDto.Email), Times.Once);
            Assert.AreEqual(ErrorMessages.MaxLoginAttemptsExceeded, ex.Message);

        }

        [TestMethod]
        public async Task GenerateNewAccessTokenAsync_WhenValidRefreshToken_ShouldReturnAccessToken()
        {
            string refreshToken = "refresh";
            string acessToken2 = "acess";
            int userId = 1;
            RefreshTokenResponseDto refreshTokenDto = new RefreshTokenResponseDto { Token = refreshToken, UserId = userId, ExpiresAt = DateTime.UtcNow.AddDays(2) };
            _mockRefreshTokenService.Setup(s => s.GetValidRefreshTokenAsync(refreshToken)).ReturnsAsync(refreshTokenDto);
            _mockJwtService.Setup(s => s.GenerateAccessToken(userId.ToString())).Returns(acessToken2);

            string acessToken = await _authUseCase.GenerateNewAccessTokenAsync(refreshToken);

            Assert.AreEqual(acessToken2, acessToken);
        }
        [TestMethod]
        public async Task GenerateNewAccessTokenAsync_WhenRefreshTokenNotExist_ShouldThrow()
        {
            string refreshToken = "token not exist in db";
            string acessToken2 = "acess";
            _mockRefreshTokenService.Setup(s => s.GetValidRefreshTokenAsync(refreshToken)).ThrowsAsync(new InvalidCredentialException(ErrorMessages.InvalidToken));

            var ex = await Assert.ThrowsExceptionAsync<InvalidCredentialException>(() => _authUseCase.GenerateNewAccessTokenAsync(refreshToken));

            Assert.AreEqual(ErrorMessages.InvalidToken, ex.Message);
        }


    }
}
