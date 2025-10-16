using Core.Dto.User;
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

        public async Task AddAsync(UserCreateRequestDto userCreateDto)
        {
            User user = new User { Email = userCreateDto.Email, Password = userCreateDto.Password, FullName = userCreateDto.FullName};
            await _userRepositoryI.AddAsync(user);
        }
    }
}
