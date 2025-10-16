using Core.Dto.Auth;
using Core.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface UserServicesI
    {
        Task AddAsync(UserCreateRequestDto userCreateDto);
        Task ValidateCredentialsAsync(LoginRequestDto loginDto);
    }
}
