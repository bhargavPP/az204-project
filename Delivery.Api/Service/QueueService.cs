using System.Text.Json;
using Azure.Storage.Queues;

namespace Delivery.Api.Service
{
    public class QueueService
    {
        private readonly string _connectionString;

        public QueueService(IConfiguration configuration)
        {
            _connectionString =
                configuration["StorageConnection"]!;
        }

        public async Task SendMessageAsync<T>(
            string queueName,
            T message)
        {
            var queueClient = new QueueClient(
                _connectionString,
                queueName);

            await queueClient.CreateIfNotExistsAsync();

            var json =
                JsonSerializer.Serialize(message);

            await queueClient.SendMessageAsync(json);
        }
    }
}
