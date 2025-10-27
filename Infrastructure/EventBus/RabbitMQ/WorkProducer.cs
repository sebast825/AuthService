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
    public class WorkProducer : IAsyncDisposable
    {
        private IConnection? _connection;
        private  IChannel? _channel;
        private WorkProducer(IConnection connection, IChannel channel)
        {
            _connection = connection;
            _channel = channel;
        }

        public static async Task<WorkProducer> CreateAsync(string hostName = "localhost")
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            var producer = new WorkProducer(connection, channel);
            await producer.SetupRabbitMQAsync();
            return producer;
        }

        private async Task SetupRabbitMQAsync()
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

        public async ValueTask DisposeAsync()
        {
            await _channel?.CloseAsync();
            await _connection?.CloseAsync();
        }
    }
}
