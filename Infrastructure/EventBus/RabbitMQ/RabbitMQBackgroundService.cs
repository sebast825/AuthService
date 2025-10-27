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
        private readonly IServiceProvider _serviceProvider;
        private RabbitMqEventConsumer? _consumer;
        public RabbitMQBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
        //curretly ont using stoppingToke, for production we need to configure this
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _consumer = await RabbitMqEventConsumer.CreateAsync();
                _consumer.ConsumirExitosos();
                _consumer.ConsumirFallidos();
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
            if (_consumer != null)
            {
                await _consumer.DisposeAsync();
            }
            await base.StopAsync(cancellationToken);
        }
    }
}
