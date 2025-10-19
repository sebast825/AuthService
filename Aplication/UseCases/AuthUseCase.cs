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
        private readonly EmailAttemptsServiceI _EmailAttemptsServiceI;
        private readonly LoginAttemptsServiceI _loginAttemptsServiceI;
        public AuthUseCase(UserServicesI userServicesI, JwtServiceI jwtServiceI, RefreshTokenServiceI refreshTokenServiceI, EmailAttemptsServiceI EmailAttemptsServiceI, LoginAttemptsServiceI loginAttemptsServiceI)
        {
            _userServicesI = userServicesI;
            _jwtServiceI = jwtServiceI;
            _refreshTokenServiceI = refreshTokenServiceI;
            _EmailAttemptsServiceI = EmailAttemptsServiceI;
            _loginAttemptsServiceI = loginAttemptsServiceI;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            UserResponseDto userResponseDto = await _userServicesI.ValidateCredentialsAsync(loginDto);
            string jwtToken =_jwtServiceI.GenerateAccessToken(userResponseDto.Id.ToString());
            RefreshToken refreshToken = _refreshTokenServiceI.CreateRefreshToken(userResponseDto.Id);
            await _refreshTokenServiceI.AddAsync(refreshToken);
            AuthResponseDto authResponseDto = new AuthResponseDto()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token,
                User = userResponseDto
            };
            return authResponseDto;
        }
        private bool HandleAttemptLogin(string email,int userId, string ip)
        {
           bool emailIsBlocked = _EmailAttemptsServiceI.EmailIsBlocked(email);
            if (emailIsBlocked) {
                _loginAttemptsServiceI.CreateLoginAttempt(userId, ip, true);
            }
        }
    }
}
