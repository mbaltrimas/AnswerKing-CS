using System;
using Answer.King.Api.RequestModels;
using Answer.King.Api.Services;
using Answer.King.Domain;
using Answer.King.Domain.Repositories;
using Answer.King.Test.Common.CustomTraits;
using NSubstitute;
using Xunit;
using Category = Answer.King.Domain.Inventory.Category;

namespace Answer.King.Api.UnitTests.Services
{
    [TestCategory(TestType.Unit)]
    public class ProductServiceTests
    {
        public ProductServiceTests()
        {
            this.ProductService = new ProductService(this.ProductRepository, this.CategoryRepository);
        }

        [Fact]
        public async void CreateProduct_InvalidCategoryThrowsException()
        {
            var productRequest = new Product
            {
                Name = "Laptop",
                Description = "desc",
                Price = 1500.00,
                Category = new CategoryId {Id = Guid.NewGuid()}
            };

            this.CategoryRepository.Get(Arg.Any<Guid>()).Returns(null as Category);

            await Assert.ThrowsAsync<ProductServiceException>(() => ProductService.CreateProduct(productRequest));
        }

        [Fact]
        public async void RetireProduct_ReturnsNullIfProductNotFound()
        {
            this.ProductRepository.Get(Arg.Any<Guid>()).Returns(null as Domain.Repositories.Models.Product);
            Assert.Null(await this.ProductService.RetireProduct(Guid.NewGuid()));
        }

        [Fact]
        public void RetireProduct_RemovesProductFromCategory()
        {
            var product = new Domain.Repositories.Models.Product(
                "product", "desc", 12.00, new Domain.Repositories.Models.Category(Guid.NewGuid(), "category", "desc"));
            this.ProductRepository.Get(product.Id).Returns(product);


            var category = new Category("category", "desc");
            this.CategoryRepository.GetByProductId(product.Id)
                .Returns(category);

            this.ProductService.RetireProduct(product.Id);

            this.CategoryRepository.Received().GetByProductId(product.Id);
            this.CategoryRepository.Save(category);
            this.ProductRepository.AddOrUpdate(product);
        }

        [Fact]
        public async void UpdateProduct_ReturnsNullIfProductCannotBeFound()
        {
            this.ProductRepository.Get(Arg.Any<Guid>()).Returns(null as Domain.Repositories.Models.Product);
            Assert.Null(await this.ProductService.UpdateProduct(Guid.NewGuid(), new Product()));
        }

        [Fact]
        public async void UpdateProduct_ThrowsExceptionIfCategoryCouldNotBeFoundForProduct()
        {
            var category = new Domain.Repositories.Models.Category(Guid.NewGuid(), "category", "desc");
            var product = new Domain.Repositories.Models.Product("product", "desc", 10.00, category);

            this.ProductRepository.Get(Arg.Any<Guid>()).Returns(product);
            this.CategoryRepository.GetByProductId(product.Id).Returns(null as Category);

            await Assert.ThrowsAsync<ProductServiceException>(() =>
                this.ProductService.UpdateProduct(product.Id, new Product()));
        }

        [Fact]
        public async void UpdateProduct_ThrowsExceptionIfUpdatedCategoryIsInvalid()
        {
            var oldCategory = new Category("category", "desc");
            var product = new Domain.Repositories.Models.Product(
                "product",
                "desc",
                10.00,
                new Domain.Repositories.Models.Category(oldCategory.Id, "category", "desc"));

            var updatedCategory = new Category("updated category", "desc");

            this.ProductRepository.Get(Arg.Any<Guid>()).Returns(product);
            this.CategoryRepository.GetByProductId(product.Id).Returns(oldCategory);
            this.CategoryRepository.Get(updatedCategory.Id).Returns(null as Category);

            var updatedProduct = new Product() {Category = new CategoryId {Id = updatedCategory.Id}};
            await Assert.ThrowsAsync<ProductServiceException>(() =>
                this.ProductService.UpdateProduct(product.Id, updatedProduct));
        }

        #region Setup

        private readonly ICategoryRepository CategoryRepository = Substitute.For<ICategoryRepository>();
        private readonly IProductRepository ProductRepository = Substitute.For<IProductRepository>();
        private readonly IProductService ProductService;

        #endregion
    }
}