using System;
using System.Collections.Generic;
using Answer.King.Api.RequestModels;
using Answer.King.Api.Services;
using Answer.King.Domain.Repositories;
using Answer.King.Test.Common.CustomTraits;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;
using Category = Answer.King.Domain.Repositories.Models.Category;
using Order = Answer.King.Domain.Orders.Order;
using Product = Answer.King.Domain.Repositories.Models.Product;

namespace Answer.King.Api.UnitTests.Services
{
    [TestCategory(TestType.Unit)]
    public class OrderServiceTests
    {
        public OrderServiceTests()
        {
            this._orderService = new OrderService(this._orderRepository, this._productRepository);
        }

        [Fact]
        public async void CreateOrder_ThrowsExceptionForSubmittingInvalidProducts()
        {
            // Arrange
            var lineItem1 = new LineItem
            {
                Product = new ProductId {Id = Guid.NewGuid()},
                Quantity = 1
            };

            var lineItem2 = new LineItem
            {
                Product = new ProductId {Id = Guid.NewGuid()},
                Quantity = 1
            };

            var orderRequest = new RequestModels.Order
            {
                LineItems = new List<LineItem>(new[] {lineItem1, lineItem2})
            };

            // Act / Assert
            await Assert.ThrowsAsync<ProductInvalidException>(() => this._orderService.CreateOrder(orderRequest));
        }


        [Fact]
        public async void CreateOrder_SuccessfullyCreatesAndSaves()
        {
            // Arrange
            var category = new Category(Guid.NewGuid(), "Cat 1", "desc");
            var products = new[]
            {
                new Product("product 1", "desc", 2.0, category),
                new Product("product 2", "desc", 4.0, category)
            };

            var orderRequest = new RequestModels.Order
            {
                LineItems = new List<LineItem>(new[]
                {
                    new LineItem {Product = new ProductId {Id = products[0].Id}, Quantity = 4},
                    new LineItem {Product = new ProductId {Id = products[1].Id}, Quantity = 1}
                })
            };

            this._productRepository.Get(Arg.Any<IList<Guid>>()).Returns(products);

            // Act
            var createdOrder = await this._orderService.CreateOrder(orderRequest);

            // Assert
            Assert.Equal(2, createdOrder.LineItems.Count);
            Assert.Equal(12.0, createdOrder.OrderTotal);
        }

        [Fact]
        public async void CancelOrder_ReturnsNullWhenInvalidOrderId()
        {
            // Arrange
            this._orderRepository.Get(Arg.Any<Guid>()).ReturnsNull();

            // Act
            var cancelOrder = await this._orderService.CancelOrder(Guid.NewGuid());

            // Assert
            Assert.Null(cancelOrder);
        }

        [Fact]
        public async void UpdateOrder_ReturnsNullIfOrderDoesntExist()
        {
            // Arrange
            this._orderRepository.Get(Arg.Any<Guid>()).ReturnsNull();

            // Act / Assert
            Assert.Null(await this._orderService.UpdateOrder(Guid.NewGuid(), new RequestModels.Order()));
        }

        [Fact]
        public async void UpdateOrder_Success()
        {
            // Arrange
            var order = new Order();
            this._orderRepository.Get(Arg.Any<Guid>()).Returns(order);

            var category = new Category(Guid.NewGuid(), "Cat 1", "desc");
            var products = new[]
            {
                new Product("product 1", "desc", 2.0, category),
                new Product("product 2", "desc", 4.0, category)
            };

            var orderRequest = new RequestModels.Order
            {
                LineItems = new List<LineItem>(new[]
                {
                    new LineItem {Product = new ProductId {Id = products[0].Id}, Quantity = 4},
                })
            };

            this._productRepository.Get(Arg.Any<IList<Guid>>()).Returns(products);

            // Act
            var updatedOrder = await this._orderService.UpdateOrder(Guid.NewGuid(), orderRequest);

            // Assert
            await this._orderRepository.Received().Save(Arg.Any<Order>());

            Assert.Equal(1, updatedOrder.LineItems.Count);
            Assert.Equal(8.0, updatedOrder.OrderTotal);
        }

        [Fact]
        public async void UpdateOrder_ThrowsExceptionIfSubmittedProductIsInvalid()
        {
            // Arrange
            var order = new Order();
            this._orderRepository.Get(Arg.Any<Guid>()).Returns(order);

            var category = new Category(Guid.NewGuid(), "Cat 1", "desc");
            var products = new[]
            {
                new Product("product 1", "desc", 2.0, category),
                new Product("product 2", "desc", 4.0, category)
            };

            var orderRequest = new RequestModels.Order
            {
                LineItems = new List<LineItem>(new[]
                {
                    new LineItem {Product = new ProductId {Id = Guid.NewGuid()}, Quantity = 4},
                })
            };

            this._productRepository.Get(Arg.Any<IList<Guid>>()).Returns(products);

            // Act / Assert
            await Assert.ThrowsAsync<ProductInvalidException>(() =>
                this._orderService.UpdateOrder(Guid.NewGuid(), orderRequest));
        }

        #region Setup

        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IOrderService _orderService;

        #endregion
    }
}