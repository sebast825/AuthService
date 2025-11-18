using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.EventBus
{
    public interface IEventProducer
    {
        Task PublishSuccessfulLoginAttemptAsync(UserLoginHistory userLoginHistory);
        Task PublishFailedLoginAttemptAsync(SecurityLoginAttempt securityAttempt);
    }
}
