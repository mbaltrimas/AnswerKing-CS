using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Api.ViewModels;
using Answer.King.Domain.Repositories;
using Answer.King.Domain.Repositories.Models;
using Microsoft.AspNetCore.Mvc;

namespace Answer.King.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        public ProductsController(IProductRepository products, ICategoryRepository categories)
        {
            this.Products = products;
            this.Categories = categories;
        }

        private IProductRepository Products { get; }
        private ICategoryRepository Categories { get; }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns></returns>
        /// /// <response code="200">When all the products have been returned.</response>
        // GET api/products
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.Products.Get());
        }

        /// <summary>
        /// Get a single product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">When the product with the provided <paramref name="id"/> has been found.</response>
        /// <response code="404">When the product with the given <paramref name="id"/> does not exist</response>
        // GET api/products/{GUID}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await this.Products.Get(id);

            if (product == null)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="createProduct"></param>
        /// <response code="201">When the product has been created.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        // POST api/products
        [HttpPost]
        [ProducesResponseType(typeof(Product), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<IActionResult> Post([FromBody] CreateProduct createProduct)
        {
            var category = await this.Categories.Get(createProduct.Category.Id);

            if (category == null)
            {
                this.ModelState.AddModelError("Category", "The provided category id is not valid.");
                return this.BadRequest(this.ModelState);
            }

            var product = new Product(
                Guid.NewGuid(),
                createProduct.Name,
                createProduct.Description,
                createProduct.Price,
                category);

            await this.Products.AddOrUpdate(product);

            return this.CreatedAtAction(nameof(Get), new { product.Id } ,product);
        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateProduct"></param>
        /// <response code="200">When the product has been updated.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        /// <response code="404">When the product with the given <paramref name="id"/> does not exist.</response>
        // PUT api/products/{GUID}
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProduct updateProduct)
        {
            var product = await this.Products.Get(id);

            if (product == null)
            {
                return this.NotFound();
            }

            var category = await this.Categories.Get(updateProduct.Category.Id);

            if (category == null)
            {
                this.ModelState.AddModelError("Category", "The provided category id is not valid.");
                return this.BadRequest(this.ModelState);
            }

            product.Name = updateProduct.Name;
            product.Description = updateProduct.Description;
            product.Price = updateProduct.Price;
            product.Category = category;

            await this.Products.AddOrUpdate(product);

            return this.Ok(product);
        }

        /// <summary>
        /// Remove an existind product.
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/products/{GUID}
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}