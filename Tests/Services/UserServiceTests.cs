using Aplication.Services;
using Core.Constants;
using Core.Dto.Auth;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Authentication;

namespace Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {

        private Mock<IUserRepository> _mockUserRepo;
        private IUserServices _userService;
        [TestInitialize]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepo.Object);
        }
        /*
         * HashPassword
          password: "string",
         hashPasword : "$2a$11$b8bLW6gG7QmCCsbr/2vy5Orfz8G5HCG7QkmBAg2cqDlu5el7CEMUW"
          */

        /*
         * EnhancedHashPassword
            var userPassword = "stringstringstring";
            string hashPasword = "$2a$12$O3yjBd5ZH9RhJoT9jEiS2OoodmEqk3A8kfkJh1zvGGHZXiPd2S.wm";
        */


        [TestMethod]
        public async Task ValidateCredentialsAsync_ValidCredentials_NotThrowException()
        {
            // Configuración específica de este test

            var userEmail = "carmelo@gmail.com";
            var userPassword = "stringstringstring";
            string hashPasword = "$2a$12$O3yjBd5ZH9RhJoT9jEiS2OoodmEqk3A8kfkJh1zvGGHZXiPd2S.wm";


            _mockUserRepo.Setup(r => r.GetByEmailAsync(userEmail))
                    .Returns(Task.FromResult(new User { Email = userEmail, Password = hashPasword }));

            LoginRequestDto loginDto = new LoginRequestDto()
            {
                Email = userEmail,
                Password = userPassword
            };

            await _userService.ValidateCredentialsAsync(loginDto);

        }

        [TestMethod]
        public async Task ValidateCredentialsAsync_UserNotExist_ThrowException()
        {
            // Configuración específica de este test

            var userEmail = "carmelo@gmail.com";
            var userPassword = "passwordpasswordpassword";

            _mockUserRepo.Setup(r => r.GetByEmailAsync(userEmail))
                    .Returns(Task.FromResult<User?>(null));

            LoginRequestDto loginDto = new LoginRequestDto()
            {
                Email = userEmail,
                Password = userPassword
            };


            var ex = await Assert.ThrowsExceptionAsync<InvalidCredentialException>(() => _userService.ValidateCredentialsAsync(loginDto));
            Assert.AreEqual(ErrorMessages.InvalidCredentials, ex.Message);
        }

        [TestMethod]
        public async Task ValidateCredentialsAsync_WrongPassword_ThrowException2()
        {
            // Configuración específica de este test

            var userEmail = "carmelo@gmail.com";
            var userPassword = "testste";
            string hashPasword = "$2a$11$b8bLW6gG7QmCCsbr/2vy5Orfz8G5HCG7QkmBAg2cqDlu5el7CEMUW";


            _mockUserRepo.Setup(r => r.GetByEmailAsync(userEmail))
                           .Returns(Task.FromResult(new User { Email = userEmail, Password = hashPasword }));

            LoginRequestDto loginDto = new LoginRequestDto()
            {
                Email = userEmail,
                Password = "userPassword"
            };


            var ex = await Assert.ThrowsExceptionAsync<InvalidCredentialException>(() => _userService.ValidateCredentialsAsync(loginDto));
            Assert.AreEqual(ErrorMessages.InvalidCredentials, ex.Message);
        }
    }
}
