using Core.Dto.UserLoginHistory;
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
        public UserLoginHistoryService(IUserLoginHistoryRepository loginAttemptRepository)
        {
            _loginAttemptRepository = loginAttemptRepository;
        }
        public async Task AddSuccessAttemptAsync(UserLoginHistory userLoginHistory)
        {
            await _loginAttemptRepository.AddAsync(userLoginHistory);
        }

        public async Task<List<UserLoginHistoryResponseDto>> GetAllByUserIdAsync(int userId)
        {
            List<UserLoginHistory> userLoginHistory = await _loginAttemptRepository.GetAllAsync(x => x.UserId == userId);

            return userLoginHistory.Select(history => new UserLoginHistoryResponseDto
            {
                Id = history.Id,
                CreatedAt = history.CreatedAt,
                UserId = history.UserId,
                IpAddress = history.IpAddress,
                DeviceInfo = history.DeviceInfo
            }).ToList();
        }
    }
}
