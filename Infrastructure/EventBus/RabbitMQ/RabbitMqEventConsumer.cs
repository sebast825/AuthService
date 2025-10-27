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
    public class RabbitMqEventConsumer 
    {
        private IChannel _channel;


        public async Task InitializeAsync(string hostName = "localhost")
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            var _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await SetupRabbitMQ();
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
    }
}