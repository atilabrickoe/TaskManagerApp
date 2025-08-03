using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TaskManagerApp.Services
{
    public class RabbitMqListenerService : IAsyncDisposable
    {
        private readonly string _queueName;
        private readonly string _hostName;
        private readonly ILogger<RabbitMqListenerService> _logger;

        private IConnection? _connection;

        public event EventHandler<string>? OnMessageReceived;
        private readonly ConnectionFactory _factory;
        public RabbitMqListenerService(IConfiguration config, ILogger<RabbitMqListenerService> logger)
        {
            _logger = logger;
            _queueName = config["RabbitMQ:QueueName"] ?? "TaskManagerQueue";
            _hostName = config["RabbitMQ:HostName"] ?? "localhost";
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task StartAsync()
        {
            var connection = await _factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "TaskManagerQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("RabbitMQ message: {msg}", json);

                    OnMessageReceived?.Invoke(this, json);

                    await Task.Yield();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming RabbitMQ message");
                }
            };

            await channel.BasicConsumeAsync("TaskManagerQueue", autoAck: true, consumer: consumer);
        }

        public ValueTask DisposeAsync()
        {
            _connection?.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}