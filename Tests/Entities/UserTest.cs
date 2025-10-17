using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Entities
{
    [TestClass]
    public class UserTest
    {


        [TestMethod]
        public void ValidateEmail_WithValidEmail_NotThrowException()
        {
            var userEmail = "carmelo@gmail.com";
            var userPassword = "passwordpasswordpassword";
            User user = new User { Email = userEmail, Password = userPassword};        

            user.Validate(); 
          
        }
        [TestMethod]
        public void ValidateEmail_WithInvalidEmail_ThrowException()
        {
            var userEmail = "carmelogmail.com";
            var userPassword = "passwordpasswordpassword";
            User user = new User { Email = userEmail, Password = userPassword };

            var ex = Assert.ThrowsExactly<FormatException>(() => user.Validate());
            Assert.AreEqual("Formato de email inválido",ex.Message);
        }
        [TestMethod]
        public void ValidatePassword_WithShortPassword_ThrowException()
        {
            var userEmail = "carmelo@gmail.com";
            var userPassword = "pass";
            User user = new User { Email = userEmail, Password = userPassword};

            var ex = Assert.ThrowsExactly<FormatException>(() => user.Validate());
            Assert.AreEqual("La contraseña debe tener al menos 8 caracteres", ex.Message);
        }

    }
}
