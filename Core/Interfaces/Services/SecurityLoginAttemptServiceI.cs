using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface SecurityLoginAttemptServiceI
    {
        Task AddFailedLoginAttemptAsync(string email, string failureReason, string ipAddrress, string deviceInfo);
    }
}
