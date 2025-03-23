using AutoMapper;
using Moq;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Resources.OrdersResources;
using PetMarketRfullApi.Sevices;
using System.Runtime.CompilerServices;
using Xunit;

namespace PetMarketRfullApi.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task Create_ShouldReturnOrderResource()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            var createOrderResource = new CreateOrderResource();
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId};

            var createdOrder = new Order { Id = orderId };
            var orderResource = new OrderResource { Id = orderId };

            //map CreateOrderResource в Order
            mockMapper.Setup(m => m.Map<Order>(It.IsAny<CreateOrderResource>())).Returns(order);

            mockUnitOfWork.Setup(u => u.Orders.AddOrderAsync(It.IsAny<Order>())).ReturnsAsync(createdOrder);

            //map Order в OrderResource
            mockMapper.Setup(m => m.Map<OrderResource>(It.IsAny<Order>())).Returns(orderResource);

            var orderService = new OrderService(mockMapper.Object, mockUnitOfWork.Object);

            //act
            var result = await orderService.Create(createOrderResource);

            //assert
            Assert.NotNull(result);
            Assert.IsType<OrderResource>(result);

            Assert.Equal(orderId, result.Id);

            mockMapper.Verify(m => m.Map<Order>(It.IsAny<CreateOrderResource>()), Times.Once());
            mockMapper.Verify(m => m.Map<OrderResource>(It.IsAny<Order>()), Times.Once());
            mockUnitOfWork.Verify(u => u.Orders.AddOrderAsync(It.IsAny<Order>()), Times.Once());
        }

    }
}
