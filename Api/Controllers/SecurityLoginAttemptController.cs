using Aplication.Services;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityLoginAttemptController : Controller
    {
        private readonly ISecurityLoginAttemptService _securityLoginAttemptService;
        public SecurityLoginAttemptController(ISecurityLoginAttemptService securityLoginAttemptService)
        {
            _securityLoginAttemptService = securityLoginAttemptService;
        }
        [HttpGet]
        public async Task<List<SecurityLoginAttempt>> GetAll()
        {
            return await _securityLoginAttemptService.GetAllAsync();
         
        }
    }
}
