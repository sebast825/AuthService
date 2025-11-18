using Core.Entities;
using Core.Interfaces.EventBus;
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
    public class RabbitMqEventProducer : IEventProducer, IAsyncDisposable
    {
        //wait the method to be called and then init
        private readonly Lazy<Task> _initializationTask;
        private IConnection? _connection;
        private  IChannel? _channel;
        public RabbitMqEventProducer()
        {
            _initializationTask = new Lazy<Task>(()=>InitAsync());
        }

        private async Task InitAsync()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }

        public async Task PublishSuccessfulLoginAttemptAsync(string message)
        {
            await _initializationTask.Value;
            var body = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync("login-exchange", "success", body);
        }

        public async Task PublishFailedLoginAttemptAsync(string message)
        {
            await _initializationTask.Value;

            var body = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync("login-exchange", "failed", body);
        }

        public async ValueTask DisposeAsync()
        {
            await _channel?.CloseAsync();
            await _connection?.CloseAsync();
        }

        public async Task PublishFailedLoginAttemptAsync(SecurityLoginAttempt securityAttempt)
        {
            await _initializationTask.Value;

            var body = Encoding.UTF8.GetBytes("message");
            await _channel.BasicPublishAsync("login-exchange", "failed", body);
        }
    }
}
