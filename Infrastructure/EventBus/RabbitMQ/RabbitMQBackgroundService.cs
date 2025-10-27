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
        public  RabbitMQBackgroundService(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider; 
        
        }
        //curretly ont using stoppingToke, for production we need to configure this
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope  = _serviceProvider.CreateScope();
            RabbitMqEventConsumer consumer = new RabbitMqEventConsumer();
            await consumer.InitializeAsync();

            // the service remain listening
            await Task.WhenAll(consumer.ConsumirFallidos(), consumer.ConsumirExitosos());
                
        }
    }
}
