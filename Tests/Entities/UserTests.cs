using Core.Constants;
using Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Entities
{
    [TestClass]
    public class UserTests
    {


        [TestMethod]
        public void Validate_WithValidEmail_NotThrowException()
        {
            var userEmail = "carmelo@gmail.com";
            var userPassword = "passwordpasswordpassword";
            User user = new User { Email = userEmail, Password = userPassword};        

            user.Validate(); 
          
        }
        [TestMethod]
        public void Validate_WithInvalidEmail_ThrowException()
        {
            var userEmail = "carmelogmail.com";
            var userPassword = "passwordpasswordpassword";
            User user = new User { Email = userEmail, Password = userPassword };

            var ex = Assert.ThrowsException<FormatException>(() => user.Validate());
            Assert.AreEqual(ErrorMessages.EmailFormat, ex.Message);
        }
        [TestMethod]
        public void Validate_WithShortPassword_ThrowException()
        {
            var userEmail = "carmelo@gmail.com";
            var userPassword = "pass";
            User user = new User { Email = userEmail, Password = userPassword};

            var ex = Assert.ThrowsException<FormatException>(() => user.Validate());
            Assert.AreEqual(ErrorMessages.PasswordLengthMin, ex.Message);
        }

    }
}
