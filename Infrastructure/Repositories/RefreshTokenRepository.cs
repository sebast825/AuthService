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
    public class RefreshTokenRepository : RefreshTokenRepositoryI
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
    }
}
