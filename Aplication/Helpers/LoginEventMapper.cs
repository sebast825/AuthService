using Core.Constants;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Helpers
{
    internal static class LoginEventMapper
    {
        public  static SecurityLoginAttempt SecurityLoginAttemptMapper(string email, string reason, string ipAddrress, string deviceInfo)
        {
            return new SecurityLoginAttempt()
            {
                Email = email,
                FailureReason = reason,
                IpAddress = ipAddrress,
                DeviceInfo = deviceInfo,
            };
    }
    }
}
