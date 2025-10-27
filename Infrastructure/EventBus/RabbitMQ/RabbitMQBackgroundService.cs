using Core.Interfaces.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EventBus.RabbitMQ
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private IEventConsumer? _consumer;
        public RabbitMQBackgroundService(IEventConsumer eventConsumer)
        {
            _consumer = eventConsumer;

        }
        //curretly ont using stoppingToke, for production we need to configure this
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _consumer.StartConsumingSuccessfulLogins();
                _consumer.StartConsumingFailedLogins();
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Cancellation requested, exit gracefully

            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_consumer != null && _consumer is IAsyncDisposable disposable)
            {
                await disposable.DisposeAsync(); // use the "disposable" variable from pattern matching
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
