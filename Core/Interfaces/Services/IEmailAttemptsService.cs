using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IEmailAttemptsService
    {
        bool EmailIsBlocked(string email);
        void ResetAttempts(string key);
        void IncrementAttempts(string key);

    }
}
