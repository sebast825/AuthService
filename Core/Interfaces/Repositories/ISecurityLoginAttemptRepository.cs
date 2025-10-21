using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ISecurityLoginAttemptRepository
    {
        Task AddAsync(SecurityLoginAttempt securityLoginAttempt);
        Task<List<SecurityLoginAttempt>> GetAllAsync();
    }
}
