using System;
using System.Threading.Tasks;
using Answer.King.Domain.Repositories;
using Answer.King.Domain.Repositories.Models;
using Microsoft.AspNetCore.Mvc;

namespace Answer.King.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        /// <param name="category"></param>
        // POST api/categories
        [HttpPost]
        public void Post([FromBody] Category category)
        {
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
        public void Cancel(Guid id)
        {
        }
    }
}
