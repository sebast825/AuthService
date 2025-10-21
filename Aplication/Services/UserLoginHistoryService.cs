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
    public class UserLoginHistoryService : IUserLoginHistoryService
    {
        private readonly IUserLoginHistoryRepository _loginAttemptRepositoryI;
        public UserLoginHistoryService(IUserLoginHistoryRepository loginAttemptRepositoryI) {
            _loginAttemptRepositoryI = loginAttemptRepositoryI;
        }
        public async Task AddSuccessAttemptAsync(int userId, string ip, string deviceInfo)
        {
            UserLoginHistory loginAttempt = new UserLoginHistory()
            {
                UserId = userId,
                IpAddress = ip,
                DeviceInfo = deviceInfo
            };
            await _loginAttemptRepositoryI.AddAsync(loginAttempt);            
        }

    }
}
