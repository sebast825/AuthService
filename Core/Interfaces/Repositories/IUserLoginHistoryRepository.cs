using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IUserLoginHistoryRepository
    {
        Task AddAsync(UserLoginHistory loginAttempt);
        Task<List<UserLoginHistory>> GetAllAsync(Expression<Func<UserLoginHistory, bool>> predicate);

    }
}
