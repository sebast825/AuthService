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
        private readonly AuthUseCase _authUseCase;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthController( AuthUseCase authUseCase,IRefreshTokenService refreshTokenService)
        {
            _authUseCase = authUseCase;
            _refreshTokenService = refreshTokenService;
        }
        [HttpPost("login")]
        public async Task<AuthResponseDto> Login(LoginRequestDto loginDto)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var deviceInfo = HttpContext.Request.Headers["User-Agent"].ToString();
            return await _authUseCase.LoginAsync(loginDto, ipAddress, deviceInfo);
        }
        [HttpGet("check")]
        [Authorize]
        public IActionResult Check()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(new { messagge = "Token Valido", userId });
        }
        [HttpPost("access-token")]
        public async Task<string> RefreshToken([FromBody] string refrehToken)
        {
            return await _authUseCase.GenerateNewAccessTokenAsync(refrehToken);

        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
            return Ok(new { message = "Logged out successfully" });
        }

    }
}
