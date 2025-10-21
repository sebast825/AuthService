using Core.Dto.User;
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
        private readonly IUserServices _userServicesI;
        public UserController(IUserServices userServicesI) {
            _userServicesI = userServicesI;

        }
        [HttpPost]
        public async Task<ActionResult>  Create([FromBody] UserCreateRequestDto userCreateDto)
        {
           
                await _userServicesI.AddAsync(userCreateDto);
                return Ok();
          
        }
    }
}
