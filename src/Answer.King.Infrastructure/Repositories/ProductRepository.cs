using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Answer.King.Domain.Repositories;
using Answer.King.Domain.Repositories.Models;
using Answer.King.Infrastructure.SeedData;

namespace Answer.King.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository()
        {
            this.Products = ProductData.Products;
        }
        private IList<Product> Products { get; }

        public Task<Product> Get(Guid id)
        {
            return Task.FromResult(this.Products.SingleOrDefault(p => p.Id == id));
        }

        public Task<IEnumerable<Product>> Get()
        {
            return Task.FromResult(this.Products as IEnumerable<Product>);
        }

        public Task AddOrUpdate(Product product)
        {
            this.Products.Add(product);
            return Task.CompletedTask;
        }
    }
}
