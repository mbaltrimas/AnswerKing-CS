using System;
using System.Threading.Tasks;
using Answer.King.Domain.Repositories;
using Answer.King.Domain.Repositories.Models;
using Microsoft.AspNetCore.Mvc;

namespace Answer.King.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController(IProductRepository products)
        {
            this.Products = products;
        }

        private IProductRepository Products { get; }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns></returns>
        // GET api/products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.Products.Get());
        }

        /// <summary>
        /// Get a single product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/products/{GUID}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return this.Ok(await this.Products.Get(id));
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="product"></param>
        // POST api/products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            await this.Products.AddOrUpdate(product);
            return this.Ok(product);
        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        // PUT api/products/{GUID}
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Product product)
        {
        }

        /// <summary>
        /// Remove an existind product.
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/products/{GUID}
        [HttpDelete("{id}")]
        public void Cancel(Guid id)
        {
        }
    }
}