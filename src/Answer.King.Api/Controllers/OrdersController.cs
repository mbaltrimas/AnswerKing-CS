using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Answer.King.Domain.Orders.Models;
using Answer.King.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Order = Answer.King.Api.RequestModels.Order;

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
        [ProducesResponseType(typeof(IEnumerable<Domain.Orders.Order>), 200)]
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
        [ProducesResponseType(typeof(Domain.Orders.Order), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            var order = await this.Orders.Get(id);
            if (order == null)
            {
                return this.NotFound();
            }

            return this.Ok(order);
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="ordereOrder"></param>
        /// <response code="201">When the order has been created.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        // POST api/orders
        [HttpPost]
        [ProducesResponseType(typeof(Domain.Orders.Order), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<IActionResult> Post([FromBody] Order createOrder)
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

            var order = new Domain.Orders.Order();

            foreach (var lineItem in createOrder.LineItems)
            {
                var product = matchingProducts.Single(p => p.Id == lineItem.Product.Id);
                var category = new Category(product.Category.Id, product.Category.Name, product.Category.Description);

                order.AddLineItem(product.Id, product.Name, product.Description, product.Price, category, lineItem.Quantity);
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
        [ProducesResponseType(typeof(Domain.Orders.Order), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id, [FromBody] Order updateOrder)
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
                    $"Product id{(invalidProducts.Count > 1 ? "s":"")} does not exist: {string.Join(',', invalidProducts)}");

                return this.BadRequest(this.ModelState);
            }

            foreach (var lineItem in updateOrder.LineItems)
            {
                var product = matchingProducts.Single(p => p.Id == lineItem.Product.Id);
                var category = new Category(product.Category.Id, product.Category.Name, product.Category.Description);

                order.AddLineItem(product.Id, product.Name, product.Description, product.Price, category, lineItem.Quantity);
            }

            await this.Orders.Save(order);

            return this.Ok(order);
        }

        /// <summary>
        /// Cancel an existind order.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">When the order has been cancelled.</response>
        /// <response code="404">When the order with the given <paramref name="id"/> does not exist.</response>
        // DELETE api/orders/{GUID}
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Domain.Orders.Order), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var order = await this.Orders.Get(id);

            if (order == null)
            {
                return this.NotFound();
            }

            try
            {
                order.CancelOrder();
                await this.Orders.Save(order);
            }
            catch (Exception)
            {
                // ignored
            }

            return this.Ok(order);
        }
    }
}

