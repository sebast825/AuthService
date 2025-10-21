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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DataContext _dataContext;
        public RefreshTokenRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task AddAsync(RefreshToken token)
        {
            await _dataContext.Set<RefreshToken>().AddAsync(token);
            await _dataContext.SaveChangesAsync();  
        }

      

        public async Task<RefreshToken?> GetAsync(Expression<Func<RefreshToken, bool>> predicate)
        {
            return await _dataContext.Set<RefreshToken>().FirstOrDefaultAsync(predicate);

        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _dataContext.Set<RefreshToken>().Update(token);
            await _dataContext.SaveChangesAsync();
        }
    }
}
