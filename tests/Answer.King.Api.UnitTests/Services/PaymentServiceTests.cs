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
        #region MakePayment

        [Fact]
        public async void MakePayment_InvalidOrderIdReceived_ThrowsException()
        {
            // Arrange
            this.OrderRepository.Get(Arg.Any<Guid>()).ReturnsNull();

            // Act / Assert
            await Assert.ThrowsAsync<PaymentServiceException>(() =>
                this.GetServiceUnderTest().MakePayment(new MakePayment()));
        }

        [Fact]
        public async void MakePayment_PaymentAmountLessThanOrderTotal_ThrowsException()
        {
            // Arrange
            var order = new Order();
            order.AddLineItem(Guid.NewGuid(), "product", "desc", 12.00,
                new Category(Guid.NewGuid(), "category", "desc"), 2);

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 20.00};

            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

            // Act / Assert
            await Assert.ThrowsAsync<PaymentServiceException>(() =>
                this.GetServiceUnderTest().MakePayment(makePayment));
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

            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

            // Act / Assert
            await Assert.ThrowsAsync<PaymentServiceException>(() =>
                this.GetServiceUnderTest().MakePayment(makePayment));
        }

        [Fact]
        public async void MakePayment_CancelledOrder_ThrowsException()
        {
            // Arrange
            var order = new Order();
            order.CancelOrder();

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 24.00};

            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

            // Act / Assert
            await Assert.ThrowsAsync<PaymentServiceException>(() =>
                this.GetServiceUnderTest().MakePayment(makePayment));
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

            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

            // Act
            var payment = await this.GetServiceUnderTest().MakePayment(makePayment);

            // Assert
            await this.OrderRepository.Received().Save(order);
            await this.PaymentRepository.Received().Add(payment);

            Assert.Equal(expectedPayment.Amount, payment.Amount);
            Assert.Equal(expectedPayment.Change, payment.Change);
            Assert.Equal(expectedPayment.OrderTotal, payment.OrderTotal);
            Assert.Equal(expectedPayment.OrderId, payment.OrderId);
        }

        #endregion

        #region Get

        [Fact]
        public async void GetPayments_ReturnsAllPayments()
        {
            // Arrange
            var payments = new[]
            {
                new Payment(Guid.NewGuid(), 50.00, 35.00),
                new Payment(Guid.NewGuid(), 10.00, 7.95)
            };

            this.PaymentRepository.Get().Returns(payments);

            // Act
            var actualPayments = await this.GetServiceUnderTest().GetPayments();

            // Assert
            Assert.Equal(payments, actualPayments);
            await this.PaymentRepository.Received().Get();
        }

        [Fact]
        public async void GetPayment_ValidPaymentId_ReturnsPayment()
        {
            // Arrange
            var payment = new Payment(Guid.NewGuid(), 50.00, 35.00);

            this.PaymentRepository.Get(payment.Id).Returns(payment);

            // Act
            var actualPayment = await this.GetServiceUnderTest().GetPayment(payment.Id);

            // Assert
            Assert.Equal(payment, actualPayment);
            await this.PaymentRepository.Received().Get(payment.Id);
        }

        #endregion

        #region Setup

        private readonly IPaymentRepository PaymentRepository = Substitute.For<IPaymentRepository>();
        private readonly IOrderRepository OrderRepository = Substitute.For<IOrderRepository>();

        private IPaymentService GetServiceUnderTest()
        {
            return new PaymentService(this.PaymentRepository, this.OrderRepository);
        }

        #endregion
    }
}