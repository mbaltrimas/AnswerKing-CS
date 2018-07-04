using System;
using System.Threading.Tasks;
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
        public OrdersController(IOrderRepository orders)
        {
            this.Orders = orders;
        }

        private IOrderRepository Orders { get; }

        /// <summary>
        /// Get all orders.
        /// </summary>
        /// <returns></returns>
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
        // GET api/orders/{GUID}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return this.Ok(await this.Orders.Get(id));
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="order"></param>
        // POST api/orders
        [HttpPost]
        public void Post([FromBody] Order order)
        {
        }

        /// <summary>
        /// Update an existing order.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        // PUT api/orders/{GUID}
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Order order)
        {
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

