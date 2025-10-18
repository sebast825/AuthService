using Aplication.Services;
using Core.Dto.Auth;
using Core.Dto.User;
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

        public async Task<string> LoginAsync(LoginRequestDto loginDto)
        {
            UserResponseDto userResponseDto = await _userServicesI.ValidateCredentialsAsync(loginDto);
            string jwtToken =_jwtServiceI.GenerateAccessToken(userResponseDto.Id);
            return jwtToken;
        }
    }
}
