using Core.Entities;
using Core.Interfaces.EventBus;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
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
    public class RabbitMqEventConsumer : IEventConsumer, IAsyncDisposable
    {
        private IConnection _connection;
        private IChannel _channel;
        private readonly IServiceProvider _serviceProvider;
        public RabbitMqEventConsumer(IServiceProvider serviceProvider)
        {

            _serviceProvider = serviceProvider;
        }
        public async Task InitAsync(string hostName = "localhost")
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
        public async Task StartConsumingSuccessfulLogins()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                UserLoginHistory loginAttempt = JsonSerializer.Deserialize<UserLoginHistory>(message);

                using var scope = _serviceProvider.CreateScope();
                var loginHistoryService = scope.ServiceProvider.GetRequiredService<IUserLoginHistoryService>();
                await loginHistoryService.AddSuccessAttemptAsync(loginAttempt);

            };

            await _channel.BasicConsumeAsync(
                queue: "login-success-queue",
                autoAck: true,
                consumer: consumer
                );
        }

        public async Task StartConsumingFailedLogins()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                SecurityLoginAttempt loginAttempt = JsonSerializer.Deserialize<SecurityLoginAttempt>(message);

                using var scope = _serviceProvider.CreateScope();
                var securityLoginAttemptService = scope.ServiceProvider.GetRequiredService<ISecurityLoginAttemptService>();
                await securityLoginAttemptService.AddFailedLoginAttemptAsync(loginAttempt);
            };

            await _channel.BasicConsumeAsync(
                queue: "login-failed-queue",
                autoAck: true,
                consumer: consumer
                );
        }

        public async ValueTask DisposeAsync()
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }
}