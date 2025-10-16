using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface UserRepositoryI
    {
        Task AddAsync(string email, string password, string? fullname = null);

    }
}
