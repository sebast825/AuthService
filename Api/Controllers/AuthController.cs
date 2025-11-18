using Aplication.UseCases;
using Core.Dto.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var deviceInfo = HttpContext.Request.Headers["User-Agent"].ToString();
            return Ok(await _authUseCase.LoginAsync(loginDto, ipAddress, deviceInfo));
        }

        [HttpPost("access-token")]
        public async Task<ActionResult<string>> RefreshToken([FromBody] string refrehToken)
        {
            return Ok(await _authUseCase.GenerateNewAccessTokenAsync(refrehToken));

        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
            return Ok(new { message = "Logged out successfully" });
        }

    }
}
