using Core.Dto.UserLoginHistory;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLoginHistoryController : Controller
    {
        private readonly IUserLoginHistoryService _userLoginHistoryService;
        public UserLoginHistoryController(IUserLoginHistoryService userLoginHistoryService)
        {
            _userLoginHistoryService = userLoginHistoryService;
        }
        [HttpGet("id")]
        public async Task<ActionResult<List<UserLoginHistoryResponseDto>>> GetById(int id)
        {
            return Ok(await _userLoginHistoryService.GetAllByUserIdAsync(id));
            
        }
    }
}
