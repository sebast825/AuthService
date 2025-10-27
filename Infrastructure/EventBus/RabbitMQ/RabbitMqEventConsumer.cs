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
    public class RabbitMqEventConsumer : IAsyncDisposable
    {
        private IConnection? _connection;
        private IChannel? _channel;

        private RabbitMqEventConsumer(IConnection connection, IChannel channel)
        {
            _connection = connection;
            _channel = channel;
        }
        public static async Task<RabbitMqEventConsumer> CreateAsync(string hostName = "localhost")
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            var consumer = new RabbitMqEventConsumer( connection,channel);
            await consumer.SetupRabbitMQ();
            return consumer;
        }

    

        private async Task SetupRabbitMQ()
        {
            // Exchange
            await _channel.ExchangeDeclareAsync("login-exchange", ExchangeType.Direct);

            // queue
            await _channel.QueueDeclareAsync("login-success-queue", durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueDeclareAsync("login-failed-queue", durable: true, exclusive: false, autoDelete: false);

            // Bindings
            await _channel.QueueBindAsync("login-success-queue", "login-exchange", "success");
            await _channel.QueueBindAsync("login-failed-queue", "login-exchange", "failed");
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

        public async ValueTask DisposeAsync()
        {
            await _channel?.CloseAsync();
            await _connection?.CloseAsync();
        }
    }
}