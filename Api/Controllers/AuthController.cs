using Aplication.Services;
using Aplication.UseCases;
using Core.Dto.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserServicesI _userServiceI;
        private readonly AuthUseCase _authUseCase;
        public AuthController(UserServicesI userServiceI, AuthUseCase authUseCase)
        {
            _userServiceI = userServiceI;
            _authUseCase = authUseCase;
        }
        [HttpPost("login")]
        public async Task<string> Login(LoginRequestDto loginDto)
        {
            //await _userServiceI.ValidateCredentialsAsync(loginDto);
            return await _authUseCase.Auth(loginDto);
           // return Ok();
        }

        /* [HttpPost("logout")]
         public async Task<IActionResult> Logout()


         [HttpPost("refresh-token")]
             public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)


         [HttpPost("forgot-password")]
             public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        */
    }
}
