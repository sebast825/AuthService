using Aplication.Services;
using Aplication.UseCases;
using Core.Dto.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<AuthResponseDto> Login(LoginRequestDto loginDto)
        {
            //await _userServiceI.ValidateCredentialsAsync(loginDto);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var deviceInfo = HttpContext.Request.Headers["User-Agent"].ToString();
            return await _authUseCase.LoginAsync(loginDto, ipAddress, deviceInfo);
           // return Ok();
        }
        [HttpGet("check")]
        [Authorize]
        public IActionResult Check()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(new { messagge = "Token Valido", userId });
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
