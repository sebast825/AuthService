using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class UserService : UserServicesI
    {
        private readonly UserRepositoryI _userRepositoryI;
        public UserService(UserRepositoryI userRepositoryI) {

            _userRepositoryI = userRepositoryI;
        }

        public async Task AddAsync(string email, string password, string? fullname = null)
        {
            User user = new User { Email = email, Password = password, FullName = fullname };
            await _userRepositoryI.AddAsync(user);
        }
    }
}
