using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface UserLoginHistoryServiceI
    {
        Task AddSuccessAttemptAsync(int userId, string ip,string deviceInfo);

    }
}
