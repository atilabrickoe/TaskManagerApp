using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Helpers;

namespace TaskManagerApp.Services
{
    public class ReceiveNotificationService : IReceiveNotificationService
    {
        private readonly ConnectionFactory _factory;
        private string Message { get; set; }
        public ReceiveNotificationService()
        {
            _factory = new ConnectionFactory { HostName = "localhost" };
        }
        public async Task ReceiveNotificationAsync()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "TaskManagerQueue", durable: false, exclusive: false, autoDelete: false,

                arguments: null);


            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                Message = Encoding.UTF8.GetString(body);
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("TaskManagerQueue", autoAck: true, consumer: consumer);
            NotificationHelper.ShowNotification("Nova Notificação", Message);
        }
    }

}
