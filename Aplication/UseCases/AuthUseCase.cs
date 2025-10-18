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
        private readonly RefreshTokenServiceI _refreshTokenServiceI;
        public AuthUseCase(UserServicesI userServicesI, JwtServiceI jwtServiceI, RefreshTokenServiceI refreshTokenServiceI  )
        {
            _userServicesI = userServicesI;
            _jwtServiceI = jwtServiceI;
            _refreshTokenServiceI = refreshTokenServiceI;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            UserResponseDto userResponseDto = await _userServicesI.ValidateCredentialsAsync(loginDto);
            string jwtToken =_jwtServiceI.GenerateAccessToken(userResponseDto.Id.ToString());
            RefreshToken refreshToken = _refreshTokenServiceI.Create(userResponseDto.Id);
            await _refreshTokenServiceI.AddAsync(refreshToken);
            AuthResponseDto authResponseDto = new AuthResponseDto()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token,
                User = userResponseDto


            };
            return authResponseDto;
        }

    }
}
