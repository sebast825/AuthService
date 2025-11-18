using Core.Dto.SecurityLoginAttempt;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ISecurityLoginAttemptService
    {
        Task AddFailedLoginAttemptAsync(SecurityLoginAttempt attempt);
        Task<List<SecurityLoginAttemptResponseDto>> GetAllAsync();
    }
}
