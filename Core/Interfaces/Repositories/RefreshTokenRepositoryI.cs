using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface RefreshTokenRepositoryI
    {
        Task AddAsync (RefreshToken token);
        Task<RefreshToken?> GetAsync(Expression<Func<RefreshToken, bool>> predicate);


    }
}
