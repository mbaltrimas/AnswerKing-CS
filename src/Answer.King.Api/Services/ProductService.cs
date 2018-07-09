using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Domain.Inventory.Models;
using Answer.King.Domain.Repositories;
using Answer.King.Domain.Repositories.Models;

namespace Answer.King.Api.Services
{
    public class ProductService : IProductService
    {
        public ProductService(
            IProductRepository products,
            ICategoryRepository categories)
        {
            this.Products = products;
            this.Categories = categories;
        }

        private IProductRepository Products { get; }

        private ICategoryRepository Categories { get; }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await this.Products.Get();
        }

        public async Task<IEnumerable<Product>> GetProducts(IEnumerable<Guid> productIds)
        {
            return await this.Products.Get(productIds);
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            return await this.Products.Get(productId);
        }

        public async Task<Product> CreateProduct(RequestModels.Product createProduct)
        {
            var category = await this.Categories.Get(createProduct.Category.Id);

            if (category == null)
            {
                throw new ProductServiceException("The provided category id is not valid.");
            }

            var product = new Product(
                createProduct.Name,
                createProduct.Description,
                createProduct.Price,
                new Category(category.Id, category.Name, category.Description));

            await this.Products.AddOrUpdate(product);

            return product;
        }

        public async Task<Product> UpdateProduct(Guid productId, RequestModels.Product updateProduct)
        {
            var product = await this.Products.Get(productId);

            if (product == null)
            {
                return null;
            }

            var oldCategory = await this.Categories.GetByProductId(productId);

            oldCategory?.RemoveProduct(new ProductId(productId));

            var category = await this.Categories.Get(updateProduct.Category.Id);

            if (category == null)
            {
                throw new ProductServiceException("The provided category id is not valid.");
            }

            category.AddProduct(new ProductId(productId));

            product.Name = updateProduct.Name;
            product.Description = updateProduct.Description;
            product.Price = updateProduct.Price;
            product.Category = new Category(category.Id, category.Name, category.Description);

            await this.Products.AddOrUpdate(product);

            return product;
        }

        public async Task<Product> RetireProduct(Guid productId)
        {
            var product = await this.Products.Get(productId);

            if (product == null)
            {
                return null;
            }

            var category = await this.Categories.GetByProductId(productId);
            if (category != null)
            {
                category.RemoveProduct(new ProductId(productId));
                await this.Categories.Save(category);
            }

            product.Retire();

            await this.Products.AddOrUpdate(product);

            return product;
        }
    }

    [Serializable]
    internal class ProductServiceException : Exception
    {
        public ProductServiceException(string message) : base(message)
        {
        }
    }
}
