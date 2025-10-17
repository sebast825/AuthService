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
      
    }
}
