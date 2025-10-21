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
        private readonly IUserLoginHistoryRepository _loginAttemptRepository;
        public UserLoginHistoryService(IUserLoginHistoryRepository loginAttemptRepository) {
            _loginAttemptRepository = loginAttemptRepository;
        }
        public async Task AddSuccessAttemptAsync(int userId, string ip, string deviceInfo)
        {
            UserLoginHistory loginAttempt = new UserLoginHistory()
            {
                UserId = userId,
                IpAddress = ip,
                DeviceInfo = deviceInfo
            };
            await _loginAttemptRepository.AddAsync(loginAttempt);            
        }

        public async Task<List<UserLoginHistory>> GetAllByUserIdAsync(int userId)
        {
            return await _loginAttemptRepository.GetAllAsync(x => x.UserId == userId);
        }
    }
}
