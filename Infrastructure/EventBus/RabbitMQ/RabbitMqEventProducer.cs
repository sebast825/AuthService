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
    public class RabbitMqEventProducer : IAsyncDisposable
    {
        private IConnection? _connection;
        private  IChannel? _channel;
        private RabbitMqEventProducer(IConnection connection, IChannel channel)
        {
            _connection = connection;
            _channel = channel;
        }

        public static async Task<RabbitMqEventProducer> CreateAsync(string hostName = "localhost")
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            var producer = new RabbitMqEventProducer(connection, channel);
            return producer;
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
