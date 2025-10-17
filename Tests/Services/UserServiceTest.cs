using Aplication.Services;
using Core.Dto.Auth;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {

        private Mock<UserRepositoryI> _mockUserRepo;
        private UserServicesI _userService;
        [TestInitialize]
        public void Setup()
        {
            _mockUserRepo = new Mock<UserRepositoryI>();
            _userService = new UserService(_mockUserRepo.Object);
        }

        [TestMethod]
        public void ValidateCredentialsAsync_ValidCredentials_NotThrowException()
        {
            // Configuración específica de este test

            var userEmail = "carmelo@gmail.com";
            var userPassword = "passwordpasswordpassword";
            
            _mockUserRepo.Setup(r => r.GetByEmailAsync(userEmail))
                    .Returns(Task.FromResult(new User { Email = userEmail, Password = userPassword }));

            LoginRequestDto loginDto = new LoginRequestDto()
            {
                Email = userEmail,
                Password = userPassword
            };
            _userService.ValidateCredentialsAsync(loginDto);
        }
    }
}
