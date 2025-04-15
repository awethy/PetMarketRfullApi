using RabbitMQ.Client;

namespace PetMarketRfullApi.Application.Abstractions
{
    public interface IRabbitMqChannelFactory
    {
        Task<IChannel> CreateChannelAsync();
    }
}
