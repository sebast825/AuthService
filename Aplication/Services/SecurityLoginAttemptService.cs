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
        public async Task AddFailedLoginAttemptAsync(string email, string failureReason, string ipAddrress, string deviceInfo)
        {
            SecurityLoginAttempt securityLoginAttempt = new SecurityLoginAttempt()
            {
                Email = email,
                FailureReason = failureReason,
                IpAddress = ipAddrress,
                DeviceInfo = deviceInfo

            };

            await _securityLoginAttemptRepository.AddAsync(securityLoginAttempt);
        }
        public async Task<List<SecurityLoginAttemptResponseDto>> GetAllAsync()
        {
            List<SecurityLoginAttempt> securityLoginAttemptList = await _securityLoginAttemptRepository.GetAllAsync();


            return securityLoginAttemptList.Select(attempt => new SecurityLoginAttemptResponseDto
            {
                Id = attempt.Id,
                CreatedAt = attempt.CreatedAt,
                IpAddress = attempt.IpAddress,
                DeviceInfo = attempt.DeviceInfo,
                Email = attempt.Email,
                FailureReason = attempt.FailureReason
            }).ToList();
        }
    }
}
