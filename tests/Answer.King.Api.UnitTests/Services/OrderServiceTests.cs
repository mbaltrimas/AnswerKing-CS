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
            this.OrderService = new OrderService(this.OrderRepository, this.ProductRepository);
        }

        [Fact]
        public async void CreateOrder_ThrowsExceptionForSubmittingInvalidProducts()
        {
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();

            var lineItem1 = new LineItem
            {
                Product = new ProductId {Id = productId1},
                Quantity = 1
            };

            var lineItem2 = new LineItem
            {
                Product = new ProductId {Id = productId2},
                Quantity = 1
            };

            var orderRequest = new RequestModels.Order()
            {
                LineItems = new List<LineItem>(new[] {lineItem1, lineItem2})
            };

            await Assert.ThrowsAsync<ProductInvalidException>(() => OrderService.CreateOrder(orderRequest));
        }


        [Fact]
        public async void CreateOrder_SuccessfullyCreatesAndSaves()
        {
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

            this.ProductRepository.Get(Arg.Any<IList<Guid>>()).Returns(products);

            var createdOrder = await this.OrderService.CreateOrder(orderRequest);

            Assert.Equal(2, createdOrder.LineItems.Count);
            Assert.Equal(12.0, createdOrder.OrderTotal);
        }

        [Fact]
        public async void CancelOrder_ReturnsNullWhenInvalidOrderId()
        {
            this.OrderRepository.Get(Arg.Any<Guid>()).ReturnsNull();

            var cancelOrder = await OrderService.CancelOrder(Guid.NewGuid());
            Assert.Null(cancelOrder);
        }

        [Fact]
        public async void UpdateOrder_ReturnsNullIfOrderDoesntExist()
        {
            this.OrderRepository.Get(Arg.Any<Guid>()).ReturnsNull();
            Assert.Null(await this.OrderService.UpdateOrder(Guid.NewGuid(), new RequestModels.Order()));
        }

        [Fact]
        public async void UpdateOrder_Success()
        {
            var order = new Order();
            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

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

            this.ProductRepository.Get(Arg.Any<IList<Guid>>()).Returns(products);

            var updatedOrder = await this.OrderService.UpdateOrder(Guid.NewGuid(), orderRequest);

            await this.OrderRepository.Received().Save(Arg.Any<Order>());

            Assert.Equal(1, updatedOrder.LineItems.Count);
            Assert.Equal(8.0, updatedOrder.OrderTotal);
        }

        [Fact]
        public async void UpdateOrder_ThrowsExceptionIfSubmittedProductIsInvalid()
        {
            var order = new Order();
            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

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

            this.ProductRepository.Get(Arg.Any<IList<Guid>>()).Returns(products);

            await Assert.ThrowsAsync<ProductInvalidException>(() =>
                this.OrderService.UpdateOrder(Guid.NewGuid(), orderRequest));
        }

        #region Setup

        private readonly IOrderRepository OrderRepository = Substitute.For<IOrderRepository>();
        private readonly IProductRepository ProductRepository = Substitute.For<IProductRepository>();
        private readonly IOrderService OrderService;

        #endregion
    }
}