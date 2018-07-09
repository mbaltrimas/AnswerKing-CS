using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Api.RequestModels;
using Answer.King.Domain.Orders;
using Answer.King.Domain.Repositories;
using Answer.King.Domain.Repositories.Models;
using Microsoft.AspNetCore.Mvc;
using Order = Answer.King.Domain.Orders.Order;

namespace Answer.King.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PaymentsController : ControllerBase
    {
        public PaymentsController(
            IPaymentRepository payments,
            IOrderRepository orders)
        {
            this.Payments = payments;
            this.Orders = orders;
        }

        private IPaymentRepository Payments { get; }

        private IOrderRepository Orders { get; }

        /// <summary>
        /// Gets all payments
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When all the payments have been returned.</response>
        // GET: api/payments
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Payment>), 200)]
        public async Task<IActionResult> Get()
        {
            var payments = await this.Payments.Get();
            return this.Ok(payments);
        }

        /// <summary>
        /// Get a single payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">When the payment with the provided <paramref name="id"/> has been found.</response>
        /// <response code="404">When the payment with the given <paramref name="id"/> does not exist</response>
        // GET: api/payments/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Payment), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            var payment = await this.Payments.Get(id);

            if (payment == null)
            {
                return this.NotFound();
            }

            return this.Ok(payment);
        }

        /// <summary>
        /// Create a new payment.
        /// </summary>
        /// <param name="makePayment"></param>
        /// <response code="201">When the payment has been created.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        // POST api/payments
        [HttpPost]
        [ProducesResponseType(typeof(Payment), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<IActionResult> Post([FromBody] MakePayment makePayment)
        {
            var order = await this.Orders.Get(makePayment.OrderId);

            if (order == null)
            {
                this.ModelState.AddModelError("OrderId", $"No order found for given order id: {makePayment.OrderId}.");
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var payment = new Payment(order.Id, makePayment.Amount, order.OrderTotal);

                order.CompleteOrder();

                await this.Orders.Save(order);
                await this.Payments.Add(payment);

                return this.CreatedAtAction(nameof(Get), new { payment.Id }, payment);

            }
            catch (PaymentException ex)
            {
                this.ModelState.AddModelError("Amount", ex.Message);
                return this.BadRequest(this.ModelState);
            }
            catch (OrderLifeCycleException ex)
            {
                var msg = ex.Message.ToLower().Contains("paid")
                    ? "Cannot make payment as order has already been paid."
                    : "Cannot make payment as order is cancelled.";

                this.ModelState.AddModelError("OrderId", msg);
                return this.BadRequest(this.ModelState);
            }
        }

        /// <summary>
        /// Get order for payment.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">When the order has been returned.</response>
        /// <response code="404">When the payment with the given <paramref name="id"/> does not exist.</response>
        // GET api/payments/{GUID}/order
        [HttpGet("{id}/order")]
        [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProducts(Guid id)
        {
            var payment = await this.Payments.Get(id);

            if (payment == null)
            {
                return this.NotFound();
            }

            var orderId = payment.OrderId;

            var order = await this.Orders.Get(orderId);

            return this.Ok(order);
        }
    }
}
