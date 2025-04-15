using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Domain.Options;
using RabbitMQ.Client;

namespace PetMarketRfullApi.Application.Sevices
{
    public class RabbitMqChannelFactory : IRabbitMqChannelFactory
    {
        private readonly RabbitMqOptions _options;
        private IConnection? _connection;
        private readonly ILogger<RabbitMqChannelFactory> _logger;
        //private bool _disposed;

        public RabbitMqChannelFactory(IOptions<RabbitMqOptions> rabbitMqOptions, IConnection? connection, ILogger<RabbitMqChannelFactory> logger)
        {
            _options = rabbitMqOptions.Value;
            _connection = connection;
            _logger = logger;
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            //if (_disposed)
            //    throw new ObjectDisposedException(nameof(RabbitMqChannelFactory));

            try
            {
                if (!_connection.IsOpen)
                {
                    _logger.LogWarning("RabbitMQ connection is closed, attempting to reopen...");
                    _connection ??= await CreateConnectionAsync();
                }

                return await _connection.CreateChannelAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create RabbitMQ channel");
                throw;
            }
        }

        private async Task<IConnection> CreateConnectionAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                Password = _options.Password,
                UserName = _options.UserName,
                VirtualHost = _options.VirtualHost
            };

            return await factory.CreateConnectionAsync();
        }

        //public async ValueTask DisposeAsync()
        //{
        //    if (_disposed) return;

        //    _disposed = true;

        //    if (_connection != null)
        //    {
        //        await _connection.CloseAsync();
        //        _connection.Dispose();
        //    }
        //}
    }
}