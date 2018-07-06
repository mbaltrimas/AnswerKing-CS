using System;
using System.Threading.Tasks;
using Answer.King.Api.ViewModels;
using Answer.King.Domain.Inventory;
using Answer.King.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Answer.King.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(ICategoryRepository categories)
        {
            this.Categories = categories;
        }

        private ICategoryRepository Categories { get; }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns></returns>
        // GET api/categories
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.Categories.Get());
        }

        /// <summary>
        /// Get a single category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/categories/{GUID}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return this.Ok(await this.Categories.Get(id));
        }

        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="createCategory"></param>
        // POST api/categories
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategory createCategory)
        {
            var category = new Category(createCategory.Name, createCategory.Description);

            await this.Categories.Save(category);

            return this.CreatedAtAction(nameof(Get), new { category.Id }, category);
        }

        /// <summary>
        /// Update an existing category.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        // PUT api/categories/{GUID}
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Category category)
        {
        }

        /// <summary>
        /// Remove an existind category.
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/categories/{GUID}
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
