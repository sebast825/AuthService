using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServicesI _userServicesI;
        public UserController(UserServicesI userServicesI) {
            _userServicesI = userServicesI;

        }
        [HttpPost]
        public async Task<ActionResult>  Create(string email, string password, string? fullname = null)
        {
           
                await _userServicesI.AddAsync(email, password, fullname);
                return Ok();
          
        }
    }
}
