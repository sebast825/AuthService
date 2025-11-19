using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserLoginHistoryRepository : IUserLoginHistoryRepository
    {
        private readonly DataContext _dataContext;
        public UserLoginHistoryRepository(DataContext dataContext) {
            _dataContext = dataContext;
        } 
        public async Task AddAsync(UserLoginHistory loginAttempt)
        {
            await _dataContext.Set<UserLoginHistory>().AddAsync(loginAttempt);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<UserLoginHistory>> GetAllAsync(Expression<Func<UserLoginHistory, bool>> predicate)
        {
            {
                return await _dataContext.Set<UserLoginHistory>()
                    .Where(predicate)
                    .AsNoTracking()
                    .ToListAsync();

            }
        }
    }
}
