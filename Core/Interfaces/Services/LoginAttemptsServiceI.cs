using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface LoginAttemptsServiceI
    {
        Task AddAsync(LoginAttempt loginAttempt);
        Task CreateLoginAttempt(int userId, string ip, bool success);
    }
}
