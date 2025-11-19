using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SecurityLoginAttemptRepository : ISecurityLoginAttemptRepository
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

        public async Task<List<SecurityLoginAttempt>> GetAllAsync()
        {
           return await _dataContext.Set<SecurityLoginAttempt>().AsNoTracking().ToListAsync();

        }
    }
}
