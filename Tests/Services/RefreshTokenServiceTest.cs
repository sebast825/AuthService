using Aplication.Services;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    [TestClass]
    public class RefreshTokenServiceTest
    {
        private Mock<RefreshTokenRepositoryI> _mockRefreshTokenRepo;
        private  RefreshTokenServiceI _refreshTokenServiceI;

        [TestInitialize]
        public void Setup()
        {
            _mockRefreshTokenRepo = new Mock<RefreshTokenRepositoryI>();
            _refreshTokenServiceI = new RefreshTokenService(_mockRefreshTokenRepo.Object);
        }

        [TestMethod]
        public void Create_ValidRefreshToken_ReturnToken()
        {

            int userId = 123;
            RefreshToken token = _refreshTokenServiceI.Create(userId);

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


    }
}
