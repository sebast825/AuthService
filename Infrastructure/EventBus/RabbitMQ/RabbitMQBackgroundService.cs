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
        private  RabbitMqEventConsumer? _consumer;
        public RabbitMQBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
        //curretly ont using stoppingToke, for production we need to configure this
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) {
                try
                {
                    _consumer = await RabbitMqEventConsumer.CreateAsync();
                    _consumer.ConsumirExitosos();
                    _consumer.ConsumirFallidos();
                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch(OperationCanceledException)
                {
                    // Cancellation requested, exit gracefully
                    break;
                }catch(Exception ex)
                {
                    if (_consumer != null)
                    {
                        await _consumer.DisposeAsync();
                    }
                    //wait 5 seconds to close the app, if recibe stoppingToken (because the app is closing) does not block the shutdown
                    await Task.Delay(5000, stoppingToken);
                }
            }


        }
    }
}
