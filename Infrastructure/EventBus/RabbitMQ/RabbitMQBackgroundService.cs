using Core.Interfaces.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.EventBus.RabbitMQ
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private IEventConsumer _consumer;
        private readonly ILogger<RabbitMQBackgroundService> _logger;
        public RabbitMQBackgroundService(IEventConsumer eventConsumer, ILogger<RabbitMQBackgroundService> logger)
        {
            _consumer = eventConsumer;
            _logger = logger;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {              
                _logger.LogInformation("Starting RabbitMQ Background Service. Preparing event consumers.");
                _consumer.StartConsumingSuccessfulLogins();
                _consumer.StartConsumingFailedLogins();
                _logger.LogInformation("Successful and failed login consumers started successfully.");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Cancellation requested, exit gracefully
                _logger.LogInformation("Cancellation requested. Stopping service gracefully.");


            }
            catch (Exception ex)
            {
                _logger.LogInformation("Critical error in RabbitMQ Background Service: { Exception.Message}. Restarting or stopping service.");

                throw;

            }
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping RabbitMQ Background Service. Releasing resources.");

            if (_consumer != null && _consumer is IAsyncDisposable disposable)
            {
                await disposable.DisposeAsync(); // use the "disposable" variable from pattern matching
                _logger.LogInformation("RabbitMQ resources released successfully.");

            }

            await base.StopAsync(cancellationToken);
        }
    }
}
