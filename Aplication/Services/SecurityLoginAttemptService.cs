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
    public class SecurityLoginAttemptService : SecurityLoginAttemptServiceI
    {
        private readonly SecurityLoginAttemptRepositoryI _securityLoginAttemptRepositoryI;

        public SecurityLoginAttemptService(SecurityLoginAttemptRepositoryI securityLoginAttemptRepositoryI)
        {
            _securityLoginAttemptRepositoryI = securityLoginAttemptRepositoryI;
        }
        public async Task AddFailedLoginAttemptAsync(string email,string failureReason, string ipAddrress, string deviceInfo)
        {
            SecurityLoginAttempt securityLoginAttempt = new SecurityLoginAttempt()
            {
                Email = email,
                FailureReason = failureReason,
                IpAddress = ipAddrress,
                DeviceInfo = deviceInfo

            };

            await _securityLoginAttemptRepositoryI.AddAsync(securityLoginAttempt);
        }
    }
}
