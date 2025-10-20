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
    public class SecurityLoginAttemptRepository : SecurityLoginAttemptRepositoryI
    {
        private readonly DataContext _dataContext;

        public SecurityLoginAttemptRepository(DataContext dataContext)
        {
        _dataContext = dataContext;
        }
        public async Task AddAsync(SecurityLoginAttempt securityLoginAttempt)
        {
            await _dataContext.Set<SecurityLoginAttempt>().AddAsync(securityLoginAttempt);
            await _dataContext.SaveChangesAsync();
        }
    }
}
