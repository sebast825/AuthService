using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.EventBus
{
    public interface IEventConsumer
    {
        Task StartConsumingSuccessfulLogins();
        Task StartConsumingFailedLogins();

    }
}
