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
    public class LoginAttemptRepository : LoginAttemptRepositoryI
    {
        private readonly DataContext _dataContext;
        public LoginAttemptRepository(DataContext dataContext) {
            _dataContext = dataContext;
        } 
        public async Task AddAsync(LoginAttempt loginAttempt)
        {
            await _dataContext.Set<LoginAttempt>().AddAsync(loginAttempt);
            await _dataContext.SaveChangesAsync();
        }
    }
}
