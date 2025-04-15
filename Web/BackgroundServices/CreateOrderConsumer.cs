using Microsoft.Extensions.Options;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.OrdersResources;
using PetMarketRfullApi.Domain.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PetMarketRfullApi.Web.BackgroundServices
{
    public class CreateOrderConsumer : BackgroundService
    {
        private readonly RabbitMqOptions _options;
        private readonly IRabbitMqChannelFactory _channelFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CreateOrderConsumer> _logger;
        private IChannel? _channel;

        public CreateOrderConsumer(
            IOptions<RabbitMqOptions> options,
            IServiceProvider serviceProvider,
            IRabbitMqChannelFactory channelFactory,
            ILogger<CreateOrderConsumer> logger)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _channelFactory = channelFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            _channel = await _channelFactory.CreateChannelAsync();


            // Объявляем очередь (если не существует)
            await _channel.QueueDeclareAsync(
                queue: _options.CreateOrderQueueName,  // Имя очереди
                durable: true,      // Сохранять сообщения при перезапуске
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Создаем асинхронного потребителя
            var consumer = new AsyncEventingBasicConsumer(_channel);

            // Создаем асинхронного потребителя
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Received order creation message: {Message}", message);

                    var createOrderResource = JsonSerializer.Deserialize<CreateOrderResource>(message, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }!);
                    using var scope = _serviceProvider.CreateScope();
                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                    await orderService.Create(createOrderResource!);

                    // Подтверждаем обработку сообщения
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"{ex.Message}. JSON deserialization error");
                    // Отклоняем сообщение без возврата в очередь
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                }
                catch (Exception ex) {
                    _logger.LogError(ex, "Order processing failed");
                    // Отклоняем сообщение с возвратом в очередь
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);  
                }
            };

            _logger.LogInformation("Потребление сообщения началось!Started!Started!Started!Started!Started!Started!Started!");
            // Начинаем потребление сообщений
            await _channel.BasicConsumeAsync(_options.CreateOrderQueueName, autoAck: false, consumer, cancellationToken: token);
        }

        // Метод остановки сервиса
        public override async Task StopAsync(CancellationToken token)
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }

            await base.StopAsync(token);
        }
    }
}
