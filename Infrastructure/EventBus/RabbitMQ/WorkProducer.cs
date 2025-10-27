using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Abstractions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EventBus.RabbitMQ
{
    public class WorkProducer
    {
        private IConnection? _connection;
        private  IChannel? _channel;
        public async Task InitializeAsync(string hostName = "localhost")
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = await factory.CreateConnectionAsync();
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

        public async Task PublicarLoginExitoso(string mensaje)
        {
            var body = Encoding.UTF8.GetBytes(mensaje);
            await _channel.BasicPublishAsync("login-exchange", "success", body);
        }

        public async Task PublicarLoginFallido(string mensaje)
        {
            var body = Encoding.UTF8.GetBytes(mensaje);
            await _channel.BasicPublishAsync("login-exchange", "failed", body);
        }
    }
}
