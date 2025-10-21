using Aplication.Services;
using Core.Constants;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    [TestClass]
    public class RefreshTokenServiceTest
    {
        private Mock<IRefreshTokenRepository> _mockRefreshTokenRepo;
        private  IRefreshTokenService _refreshTokenServiceI;

        [TestInitialize]
        public void Setup()
        {
            _mockRefreshTokenRepo = new Mock<IRefreshTokenRepository>();
            _refreshTokenServiceI = new RefreshTokenService(_mockRefreshTokenRepo.Object);
        }

        [TestMethod]
        public void CreateRefreshToken_ValidRefreshToken_ReturnToken()
        {

            int userId = 123;
            RefreshToken token = _refreshTokenServiceI.CreateRefreshToken(userId);

            Assert.IsNotNull(token);
            Assert.AreEqual(token.UserId, userId);
            Assert.IsFalse(string.IsNullOrWhiteSpace(token.Token));
            Assert.IsTrue(token.ExpiresAt >  DateTime.UtcNow);
            Assert.IsTrue(token.ExpiresAt < DateTime.UtcNow.AddDays(1).AddSeconds(5));
        }

        [TestMethod]
        public async Task RevokeRefreshToken_ValidToken_SetsRevokedTrue()
        {
            string token = "abc";
            int userId = 1;
            RefreshToken refreshToken = new RefreshToken() { Token = token, UserId = userId};

            _mockRefreshTokenRepo
                 .Setup(r => r.GetAsync(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
                 .ReturnsAsync(refreshToken);
            _mockRefreshTokenRepo
                .Setup(r => r.UpdateAsync(refreshToken))
                .Returns(Task.CompletedTask);


            await _refreshTokenServiceI.RevokeRefreshToken(refreshToken.Token);

            Assert.IsTrue(refreshToken.Revoked);
            
        }
        [TestMethod]
        public async Task RevokeRefreshToken_InvalidToken_ThrowException()
        {
            string token = "abc";

            _mockRefreshTokenRepo
                 .Setup(r => r.GetAsync(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
                 .ReturnsAsync((RefreshToken?)null);
  
    
            var ex = await Assert.ThrowsExceptionAsync<InvalidCredentialException>(() => _refreshTokenServiceI.RevokeRefreshToken(token));
            Assert.AreEqual(ErrorMessages.InvalidToken, ex.Message);

        }


    }
}
