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
            var userName = "Manolo Perez";
            var userEmail = "carmelo@gmail.com";
            var userPassword = "passwordpasswordpassword";
            User user = new User { Email = userEmail, Password = userPassword, FullName= userName };
        

            user.Validate(); 
          
        }
        [TestMethod]
        public void ValidateEmail_WithInvalidEmail_ThrowException()
        {
            var userName = "Manolo Perez";
            var userEmail = "carmelogmail.com";
            var userPassword = "passwordpasswordpassword";
            User user = new User { Email = userEmail, Password = userPassword, FullName = userName };



            var ex = Assert.ThrowsExactly<FormatException>(() => user.Validate());
            Assert.AreEqual("Formato de email inválido",ex.Message);
        }

    }
}
