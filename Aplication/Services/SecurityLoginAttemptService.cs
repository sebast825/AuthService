using Core.Constants;
using Core.Dto.SecurityLoginAttempt;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class SecurityLoginAttemptService : ISecurityLoginAttemptService
    {
        private readonly ISecurityLoginAttemptRepository _securityLoginAttemptRepository;

        public SecurityLoginAttemptService(ISecurityLoginAttemptRepository securityLoginAttemptRepository)
        {
            _securityLoginAttemptRepository = securityLoginAttemptRepository;
        }
        public async Task AddFailedLoginAttemptAsync(SecurityLoginAttempt attempt)
        {
            await _securityLoginAttemptRepository.AddAsync(attempt);
        }
        public async Task<List<SecurityLoginAttemptResponseDto>> GetAllAsync()
        {
            List<SecurityLoginAttempt> securityLoginAttemptList = await _securityLoginAttemptRepository.GetAllAsync();

            return securityLoginAttemptList.Select(attempt =>

                new SecurityLoginAttemptResponseDto
                {
                    Id = attempt.Id,
                    CreatedAt = attempt.CreatedAt,
                    IpAddress = attempt.IpAddress,
                    DeviceInfo = attempt.DeviceInfo,
                    Email = attempt.Email,
                    FailureReason = attempt.FailureReason
                }
            ).ToList();
        }
    }
}
