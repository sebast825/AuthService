using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.Repositories
{
    public class UserRepository : UserRepositoryI
    {
        private readonly DataContext _dataContext;
        public UserRepository(DataContext dataContext) {
            _dataContext = dataContext;
        }
        public async Task AddAsync(User user)
        {
            
            await _dataContext.Set<User>().AddAsync(user);
            await _dataContext.SaveChangesAsync();
        }
    }
}
