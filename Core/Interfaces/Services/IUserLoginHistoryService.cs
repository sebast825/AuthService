using Core.Dto.UserLoginHistory;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IUserLoginHistoryService
    {
        Task AddSuccessAttemptAsync(UserLoginHistory userLoginHistory);
        Task<List<UserLoginHistoryResponseDto>> GetAllByUserIdAsync(int userId);

    }
}
