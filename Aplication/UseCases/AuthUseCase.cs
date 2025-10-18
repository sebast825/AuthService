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
        private readonly JwtServiceI _jwtServiceI;
        public AuthUseCase(UserServicesI userServicesI, JwtServiceI jwtServiceI)
        {
            _userServicesI = userServicesI;
            _jwtServiceI = jwtServiceI;
        }

        public async Task<string> Auth(LoginRequestDto loginDto)
        {
            await _userServicesI.ValidateCredentialsAsync(loginDto);
            string jwtToken =_jwtServiceI.GenerateAccessToken("1");
            return jwtToken;
        }
    }
}
