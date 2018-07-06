using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Answer.King.Api.ViewModels;
using Answer.King.Domain.Orders;
using Answer.King.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Answer.King.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        public OrdersController(
            IOrderRepository orders,
            IProductRepository products)
        {
            this.Orders = orders;
            this.Products = products;
        }

        private IOrderRepository Orders { get; }

        private IProductRepository Products { get; }

        /// <summary>
        /// Get all orders.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When all the orders have been returned.</response>
        // GET api/orders
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.Orders.Get());
        }

        /// <summary>
        /// Get a single order.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">When the order with the provided <paramref name="id"/> has been found.</response>
        /// <response code="404">When the order with the given <paramref name="id"/> does not exist</response>
        // GET api/orders/{GUID}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            return this.Ok(await this.Orders.Get(id));
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="createOrder"></param>
        /// <response code="201">When the order has been created.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        // POST api/orders
        [HttpPost]
        [ProducesResponseType(typeof(Order), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<IActionResult> Post([FromBody] CreateOrder createOrder)
        {
            var submittedProductIds = createOrder.LineItems.Select(l => l.Product.Id).ToList();

            var matchingProducts =
                (await this.Products.Get(submittedProductIds)).ToList();

            var invalidProducts =
                submittedProductIds.Except(matchingProducts.Select(p => p.Id))
                    .ToList();

            if (invalidProducts.Any())
            {
                this.ModelState.AddModelError(
                    "LineItems.ProductId",
                    $"Product id{(invalidProducts.Count() > 1 ? "s" : "")} does not exist: {string.Join(',', invalidProducts)}");
                return this.BadRequest(this.ModelState);
            }

            var order = new Order();

            foreach (var lineItem in createOrder.LineItems)
            {
                var product = matchingProducts.Single(p => p.Id == lineItem.Product.Id);
                order.AddLineItem(product.Id, product.Price, lineItem.Quantity);
            }

            await this.Orders.Save(order);

            return this.CreatedAtAction(nameof(Get), new { order.Id }, order);
        }

        /// <summary>
        /// Update an existing order.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateOrder"></param>
        /// <response code="200">When the order has been updated.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        /// <response code="404">When the order with the given <paramref name="id"/> does not exist.</response>
        // PUT api/orders/{GUID}
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateOrder updateOrder)
        {
            var order = await this.Orders.Get(id);

            if (order == null)
            {
                return this.NotFound();
            }

            var submittedProductIds = updateOrder.LineItems.Select(l => l.Product.Id).ToList();

            var matchingProducts =
                (await this.Products.Get(submittedProductIds)).ToList();

            var invalidProducts =
                submittedProductIds.Except(matchingProducts.Select(p => p.Id))
                    .ToList();

            if (invalidProducts.Any())
            {
                this.ModelState.AddModelError(
                    "LineItems.ProductId",
                    $"Product id{(invalidProducts.Count() > 1 ? "s":"")} does not exist: {string.Join(',', invalidProducts)}");

                return this.BadRequest(this.ModelState);
            }

            foreach (var lineItem in updateOrder.LineItems)
            {
                var product = matchingProducts.Single(p => p.Id == lineItem.Product.Id);
                order.AddLineItem(product.Id, product.Price, lineItem.Quantity);
            }

            await this.Orders.Save(order);

            return this.Ok(order);
        }

        /// <summary>
        /// Cancel an existind order.
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/orders/{GUID}
        [HttpDelete("{id}")]
        public void Cancel(Guid id)
        {
        }
    }
}

