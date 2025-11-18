using Core.Constants;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("Tests")]

namespace Aplication.Helpers
{
    internal static class LoginEventMapper
    {
        public static SecurityLoginAttempt SecurityLoginAttemptMapper(string email, LoginFailureReasons reason, string ipAddrress, string deviceInfo)
        {
            return new SecurityLoginAttempt()
            {
                Email = email,
                FailureReason = reason,
                IpAddress = ipAddrress,
                DeviceInfo = deviceInfo,
            };
        }
        public static UserLoginHistory LoginHistoryMapper(int userId, string ipAddrress, string deviceInfo)
        {
            return new UserLoginHistory()
            {
                UserId = userId,
                IpAddress = ipAddrress,
                DeviceInfo = deviceInfo,
            };
        }
    }
}
