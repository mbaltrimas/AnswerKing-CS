using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Answer.King.Domain.Repositories;
using Answer.King.Domain.Repositories.Models;
using Answer.King.Infrastructure.SeedData;

namespace Answer.King.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public CategoryRepository()
        {
            this.Categories = CategoryData.Categories;
        }

        private IList<Category> Categories { get; }

        public Task<Category> Get(Guid id)
        {
            return Task.FromResult(this.Categories.SingleOrDefault(p => p.Id == id));
        }

        public Task<IEnumerable<Category>> Get()
        {
            return Task.FromResult(this.Categories as IEnumerable<Category>);
        }
    }
}