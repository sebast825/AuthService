using Aplication.Services;
using Core.Dto.Auth;
using Core.Entities;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class AuthUseCase
    {
        private readonly UserServicesI _userServicesI;
        public AuthUseCase(UserServicesI userServicesI) {
            _userServicesI = userServicesI;
        }

        public async Task Auth(LoginRequestDto loginDto)
        {
            _userServicesI.ValidateCredentialsAsync(loginDto);
            throw new NotImplementedException();
        }
    }
}
