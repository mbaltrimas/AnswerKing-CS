using System;
using Answer.King.Api.RequestModels;
using Answer.King.Api.Services;
using Answer.King.Domain.Repositories;
using Answer.King.Domain.Repositories.Models;
using Answer.King.Test.Common.CustomTraits;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;
using Category = Answer.King.Domain.Orders.Models.Category;
using Order = Answer.King.Domain.Orders.Order;

namespace Answer.King.Api.UnitTests.Services
{
    [TestCategory(TestType.Unit)]
    public class PaymentServiceTests
    {
        public PaymentServiceTests()
        {
            this._paymentService = new PaymentService(this._paymentRepository, this._orderRepository);
        }

        [Fact]
        public async void MakePayment_InvalidOrderIdReceived_ThrowsException()
        {
            // Arrange
            this._orderRepository.Get(Arg.Any<Guid>()).ReturnsNull();

            // Act / Assert
            await Assert.ThrowsAsync<PaymentServiceException>(() =>
                this._paymentService.MakePayment(new MakePayment()));
        }

        [Fact]
        public async void MakePayment_PaymentAmountLessThanOrderTotal_ThrowsException()
        {
            // Arrange
            var order = new Order();
            order.AddLineItem(Guid.NewGuid(), "product", "desc", 12.00,
                new Category(Guid.NewGuid(), "category", "desc"), 2);

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 20.00};

            this._orderRepository.Get(Arg.Any<Guid>()).Returns(order);

            // Act / Assert
            await Assert.ThrowsAsync<PaymentServiceException>(() => this._paymentService.MakePayment(makePayment));
        }

        [Fact]
        public async void MakePayment_PaidOrder_ThrowsException()
        {
            // Arrange
            var order = new Order();
            order.AddLineItem(Guid.NewGuid(), "product", "desc", 12.00,
                new Category(Guid.NewGuid(), "category", "desc"), 2);
            order.CompleteOrder();

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 24.00};

            this._orderRepository.Get(Arg.Any<Guid>()).Returns(order);

            // Act / Assert
            await Assert.ThrowsAsync<PaymentServiceException>(() => this._paymentService.MakePayment(makePayment));
        }

        [Fact]
        public async void MakePayment_CancelledOrder_ThrowsException()
        {
            // Arrange
            var order = new Order();
            order.CancelOrder();

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 24.00};

            this._orderRepository.Get(Arg.Any<Guid>()).Returns(order);

            // Act / Assert
            await Assert.ThrowsAsync<PaymentServiceException>(() => this._paymentService.MakePayment(makePayment));
        }

        [Fact]
        public async void MakePayment_ValidPaymentRequest_ReturnsPayment()
        {
            // Arrange
            var order = new Order();
            order.AddLineItem(Guid.NewGuid(), "product", "desc", 12.00,
                new Category(Guid.NewGuid(), "category", "desc"), 2);

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 24.00};
            var expectedPayment = new Payment(order.Id, makePayment.Amount, order.OrderTotal);

            this._orderRepository.Get(Arg.Any<Guid>()).Returns(order);

            // Act
            var payment = await this._paymentService.MakePayment(makePayment);

            // Assert
            await this._orderRepository.Received().Save(order);
            await this._paymentRepository.Received().Add(payment);

            Assert.Equal(expectedPayment.Amount, payment.Amount);
            Assert.Equal(expectedPayment.Change, payment.Change);
            Assert.Equal(expectedPayment.OrderTotal, payment.OrderTotal);
            Assert.Equal(expectedPayment.OrderId, payment.OrderId);
        }

        #region Setup

        private readonly IPaymentRepository _paymentRepository = Substitute.For<IPaymentRepository>();
        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
        private readonly IPaymentService _paymentService;

        #endregion
    }
}