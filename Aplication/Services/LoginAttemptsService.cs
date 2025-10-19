using Core.Entities;
using Core.Interfaces.Repositories;
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
        private readonly LoginAttemptRepositoryI _loginAttemptRepositoryI;
        public LoginAttemptsService(LoginAttemptRepositoryI loginAttemptRepositoryI) {
            _loginAttemptRepositoryI = loginAttemptRepositoryI;
        }
        public async Task AddAsync(int userId, string ip, bool success)
        {
            LoginAttempt loginAttempt = CreateLoginAttempt(userId, ip,success);
            await _loginAttemptRepositoryI.AddAsync(loginAttempt);            
        }

        private LoginAttempt CreateLoginAttempt(int userId, string ip, bool success)
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
