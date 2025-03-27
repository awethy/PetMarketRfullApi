using AutoMapper;
using Moq;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Mapping;
using PetMarketRfullApi.Application.Resources.CartsResources;
using PetMarketRfullApi.Application.Resources.OrdersResources;
using PetMarketRfullApi.Application.Sevices;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;
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
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelToResourceProfile>();
            }).CreateMapper();
            var mockICartService = new Mock<ICartService>();

            // Генерируем тестовые ID
            var cartId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            // Создаем тестовые данные с явно указанными ID
            //var cartItems = new List<CartItemResource>
            //{ new CartItemResource { PetId = 1} };
            var cartResource = new CartResource { Id = cartId, Items = new List<CartItemResource>() };

            var createOrderResource = new CreateOrderResource
            {
                Cart = cartResource
            };
            var order = new Order { Id = orderId};
            var createdOrder = new Order { Id = orderId, CartId = cartId };
            var orderResource = new OrderResource { Id = orderId };


            //map CreateOrderResource в Order
            //mockMapper.Setup(m => m.Map<Order>(
            //    It.IsAny<CreateOrderResource>(),
            //    It.IsAny<Action<IMappingOperationOptions>>()))
            //    .Callback<CreateOrderResource, Action<IMappingOperationOptions>>((src, opts) =>
            //    {
                    
            //    });


            mockICartService.Setup(c => c.CreateCartAsync(It.IsAny<CartResource>()))
                .ReturnsAsync(cartResource);

            //mockUnitOfWork.Setup(u => u.Orders.AddOrderAsync(It.IsAny<Order>())).ReturnsAsync(createdOrder);
            mockUnitOfWork.Setup(u => u.Orders.AddOrderAsync(It.Is<Order>(order => order.CartId == cartId)))
                .ReturnsAsync(createdOrder)
                .Verifiable();

            //map Order в OrderResource
            //mockMapper.Setup(m => m.Map<OrderResource>(It.IsAny<Order>()))
            //    .Returns(orderResource);

            var orderService = new OrderService(mockMapper, mockUnitOfWork.Object, mockICartService.Object);

            //act
            Console.WriteLine($"CartResource is null: {createOrderResource.Cart == null}");
            Console.WriteLine($"CartService is null: {mockICartService.Object == null}");
            var result = await orderService.Create(createOrderResource);

            //assert
            Assert.NotNull(result);
            Assert.IsType<OrderResource>(result);
            Assert.Equal(orderId, result.Id);

            //mockMapper.Verify(m => m.Map<Order>(It.IsAny<CreateOrderResource>()), Times.Once());
            //mockMapper.Verify(m => m.Map<OrderResource>(It.IsAny<Order>()), Times.Once());
            mockUnitOfWork.Verify(u => u.Orders.AddOrderAsync(It.IsAny<Order>()), Times.Once());
            mockICartService.Verify(c => c.CreateCartAsync(It.IsAny<CartResource>()), Times.Once());
        }

    }
}
