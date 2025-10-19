using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface EmailAttemptsServiceI
    {
        bool EmailIsBlocked(string email);
        void ResetAttempts(string key);
        void IncrementFailedAttempts(string key);

    }
}
