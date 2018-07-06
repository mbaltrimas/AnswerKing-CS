using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Answer.King.Domain.Inventory;
using Answer.King.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Category = Answer.King.Api.ViewModels.Category;
using Product = Answer.King.Domain.Repositories.Models.Product;

namespace Answer.King.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(ICategoryRepository categories, IProductRepository products)
        {
            this.Categories = categories;
            this.Products = products;
        }

        private ICategoryRepository Categories { get; }

        private IProductRepository Products { get; }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When all the categories have been returned.</response>
        // GET api/categories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Domain.Inventory.Category>), 200)]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.Categories.Get());
        }

        /// <summary>
        /// Get a single category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">When the category with the provided <paramref name="id"/> has been found.</response>
        /// <response code="404">When the category with the given <paramref name="id"/> does not exist</response>
        // GET api/categories/{GUID}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Domain.Inventory.Category), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            var category = await this.Categories.Get(id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.Ok(category);
        }

        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="createCategory"></param>
        /// <response code="201">When the category has been created.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        // POST api/categories
        [HttpPost]
        [ProducesResponseType(typeof(Domain.Inventory.Category), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<IActionResult> Post([FromBody] Category createCategory)
        {
            var category = new Domain.Inventory.Category(createCategory.Name, createCategory.Description);

            await this.Categories.Save(category);

            return this.CreatedAtAction(nameof(Get), new { category.Id }, category);
        }

        /// <summary>
        /// Update an existing category.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateCategory"></param>
        /// <response code="200">When the category has been updated.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        /// <response code="404">When the category with the given <paramref name="id"/> does not exist.</response>
        // PUT api/categories/{GUID}
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Domain.Inventory.Category), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id, [FromBody] Category updateCategory)
        {
            var category = await this.Categories.Get(id);
            if (category == null)
            {
                return this.NotFound();
            }

            category.Rename(updateCategory.Name, updateCategory.Description);

            await this.Categories.Save(category);

            return this.Ok(category);
        }

        /// <summary>
        /// Remove an existind category.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">When the category has been deleted.</response>
        /// <response code="400">When invalid parameters are provided.</response>
        /// <response code="404">When the category with the given <paramref name="id"/> does not exist.</response>
        // DELETE api/categories/{GUID}
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Domain.Inventory.Category), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Retire(Guid id)
        {
            var category = await this.Categories.Get(id);

            if (category == null)
            {
                return this.NotFound();
            }

            try
            {
                category.RetireCategory();
                await this.Categories.Save(category);

                return this.Ok(category);
            }
            catch (CategoryLifecycleException)
            {
                // ignored
                this.ModelState.AddModelError(
                    "Products",
                    $"Cannot remove category whilst there are still products assigned. {string.Join(',', category.Products.Select(p => p.Id))}");

                return this.BadRequest(this.ModelState);
            }
        }

        /// <summary>
        /// Get all products in a category.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">When all the products have been returned.</response>
        /// <response code="404">When the category with the given <paramref name="id"/> does not exist.</response>
        // GET api/categories/{GUID}/products
        [HttpGet("{id}/products")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProducts(Guid id)
        {
            var category = await this.Categories.Get(id);

            if (category == null)
            {
                return this.NotFound();
            }

            var productIds = category.Products.Select(p => p.Id);

            var products = await this.Products.Get(productIds);

            return this.Ok(products);
        }
    }
}
