using System;
using Answer.King.Api.RequestModels;
using Answer.King.Api.Services;
using Answer.King.Domain.Repositories;
using Answer.King.Domain.Repositories.Models;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;
using Category = Answer.King.Domain.Orders.Models.Category;
using Order = Answer.King.Domain.Orders.Order;

namespace Answer.King.Api.UnitTests.Services
{
    public class PaymentServiceTests
    {
        public PaymentServiceTests()
        {
            this.PaymentService = new PaymentService(this.PaymentRepository, this.OrderRepository);
        }

        [Fact]
        public async void MakePayment_ThrowsExceptionIfOrderCannotBeFound()
        {
            this.OrderRepository.Get(Arg.Any<Guid>()).ReturnsNull();
            await Assert.ThrowsAsync<PaymentServiceException>(() => this.PaymentService.MakePayment(new MakePayment()));
        }

        [Fact]
        public async void MakePayment_ThrowsExceptionWhenPaymentAmountIsLessThanOrderTotal()
        {
            var order = new Order();
            order.AddLineItem(Guid.NewGuid(), "product", "desc", 12.00,
                new Category(Guid.NewGuid(), "category", "desc"), 2);

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 20.00};

            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

            await Assert.ThrowsAsync<PaymentServiceException>(() => this.PaymentService.MakePayment(makePayment));
        }

        [Fact]
        public async void MakePayment_ThrowsExceptionWhenOrderIsAlreadyPaid()
        {
            var order = new Order();
            order.AddLineItem(Guid.NewGuid(), "product", "desc", 12.00,
                new Category(Guid.NewGuid(), "category", "desc"), 2);
            order.CompleteOrder();

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 24.00};

            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

            await Assert.ThrowsAsync<PaymentServiceException>(() => this.PaymentService.MakePayment(makePayment));
        }

        [Fact]
        public async void MakePayment_ThrowsExceptionWhenOrderIsCancelled()
        {
            var order = new Order();
            order.CancelOrder();

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 24.00};

            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

            await Assert.ThrowsAsync<PaymentServiceException>(() => this.PaymentService.MakePayment(makePayment));
        }

        [Fact]
        public async void MakePayment_SuccessfullyPaysOrderAndReturnsPayment()
        {
            var order = new Order();
            order.AddLineItem(Guid.NewGuid(), "product", "desc", 12.00,
                new Category(Guid.NewGuid(), "category", "desc"), 2);

            var makePayment = new MakePayment {OrderId = order.Id, Amount = 24.00};
            var expectedPayment = new Payment(order.Id, makePayment.Amount, order.OrderTotal);

            this.OrderRepository.Get(Arg.Any<Guid>()).Returns(order);

            var payment = await this.PaymentService.MakePayment(makePayment);

            await this.OrderRepository.Received().Save(order);
            await this.PaymentRepository.Received().Add(payment);

            Assert.Equal(expectedPayment.Amount, payment.Amount);
            Assert.Equal(expectedPayment.Change, payment.Change);
            Assert.Equal(expectedPayment.OrderTotal, payment.OrderTotal);
            Assert.Equal(expectedPayment.OrderId, payment.OrderId);
        }

        #region Setup

        private readonly IPaymentRepository PaymentRepository = Substitute.For<IPaymentRepository>();
        private readonly IOrderRepository OrderRepository = Substitute.For<IOrderRepository>();
        private readonly IPaymentService PaymentService;

        #endregion
    }
}