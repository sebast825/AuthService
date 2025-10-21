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
        Task AddSuccessAttemptAsync(int userId, string ip,string deviceInfo);
        Task<List<UserLoginHistory>> GetAllByUserIdAsync(int userId);

    }
}
