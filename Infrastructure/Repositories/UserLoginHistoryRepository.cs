using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserLoginHistoryRepository : UserLoginHistoryRepositoryI
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
    }
}
