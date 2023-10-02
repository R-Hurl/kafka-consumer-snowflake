using Confluent.Kafka;
using DataGenConsumer.Database.Repositories.Interfaces;

namespace DataGenConsumer
{
    // This class represents a Kafka consumer that reads messages from the "orders_json" topic
    // and inserts them into a Snowflake database using an IOrderRepository.
    public class DatagenKafkaConsumer : BackgroundService
    {
        private readonly ILogger<DatagenKafkaConsumer> _logger;
        private readonly ConsumerConfig _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DatagenKafkaConsumer(ILogger<DatagenKafkaConsumer> logger, ConsumerConfig config, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // This method is called when the hosted service is started, and reads messages from the "orders_json" topic
        // until the cancellation token is signaled. For each message, it inserts the order into the Snowflake database
        // using an IOrderRepository.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // This line of code is used to improve the performance of the application by allowing other tasks to run while the background service is waiting for an event to occur.
            // It allows the background service to yield control of the thread to other tasks in the thread pool while it is waiting for an event to occur.
            // This can improve the responsiveness of the application and reduce the likelihood of thread starvation. In .NET 8 they will fix this issue by making all async methods "hot" by default.
            await Task.Yield();

            using var consumer = new ConsumerBuilder<string, string>(_config).Build();
            consumer.Subscribe("orders_json");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var orderId = Guid.NewGuid().ToString();
                    var order = consumeResult.Message.Value;
                    _logger.LogInformation($"Received Key {orderId}; Received message: {order}");
                    using var scope = _serviceScopeFactory.CreateScope();
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    await orderRepository.InsertOrderAsync(orderId, order);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Consumer was cancelled");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming message");
                }
            }

            consumer.Close();
        }
    }
}