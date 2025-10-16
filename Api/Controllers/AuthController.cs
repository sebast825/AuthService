using Aplication.Services;
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
        public AuthController(UserServicesI userServiceI)
        {
            _userServiceI = userServiceI;
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequestDto loginDto)
        {
            await _userServiceI.ValidateCredentialsAsync(loginDto);
            return Ok();
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
