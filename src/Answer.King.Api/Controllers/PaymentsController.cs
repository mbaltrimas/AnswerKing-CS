using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Answer.King.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PaymentsController : ControllerBase
    {
        /// <summary>
        /// Gets all payments
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When all the payments have been returned.</response>
        // GET: api/payments
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            return this.Ok();
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
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            return this.Ok();
        }

        /// <summary>
        /// Create a new payment.
        /// </summary>
        /// <param name="payment"></param>
        /// <response code="201">When the category has been created.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        // POST api/payments
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<IActionResult> Post([FromBody] Payment payment)
        {
            return this.Ok();
        }
    }
}
