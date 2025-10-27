using Core.Interfaces.EventBus;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.EventBus.RabbitMQ
{
    public class WorkConsumer 
    {
        private IChannel _channel;


        public async Task InitializeAsync(string hostName = "localhost")
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            var _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }
        public async Task ConsumirExitosos()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensaje = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Login exitoso: {mensaje}");
            };

            await _channel.BasicConsumeAsync(queue: "login-success-queue",
                                autoAck: true,
                                consumer: consumer);
        }

        public async Task ConsumirFallidos()
        {
            var consumer =  new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async(model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensaje = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Login fallido: {mensaje}");
            };

           await _channel.BasicConsumeAsync(queue: "login-failed-queue",
                                autoAck: true,
                                consumer: consumer);
        }
    }
}