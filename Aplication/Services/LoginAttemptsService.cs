using Core.Entities;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class LoginAttemptsService : LoginAttemptsServiceI
    {
        public Task AddAsync(LoginAttempt loginAttempt)
        {
            throw new NotImplementedException();
        }

        public LoginAttempt CreateLoginAttempt(int userId, string ip, bool success)
        {
            LoginAttempt loginAttempt = new LoginAttempt()
            {
                UserId = userId,
                IpAddress = ip,
                Success = success
            };
            return loginAttempt;
        }
    }
}
