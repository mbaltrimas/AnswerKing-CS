using System;
using Answer.King.Api.RequestModels;
using Answer.King.Api.Services;
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
            this._productService = new ProductService(this._productRepository, this._categoryRepository);
        }

        [Fact]
        public async void CreateProduct_InvalidCategoryIdInProduct_ThrowsException()
        {
            // Arrange
            var productRequest = new Product
            {
                Name = "Laptop",
                Description = "desc",
                Price = 1500.00,
                Category = new CategoryId {Id = Guid.NewGuid()}
            };

            // Act
            this._categoryRepository.Get(Arg.Any<Guid>()).Returns(null as Category);

            // Assert
            await Assert.ThrowsAsync<ProductServiceException>(() => this._productService.CreateProduct(productRequest));
        }

        [Fact]
        public async void RetireProduct_InvalidProductId_ReturnsNull()
        {
            // Arrange
            this._productRepository.Get(Arg.Any<Guid>()).Returns(null as Domain.Repositories.Models.Product);
            
            // Act / Assert
            Assert.Null(await this._productService.RetireProduct(Guid.NewGuid()));
        }

        [Fact]
        public async void RetireProduct_ValidProductId_ReturnsProductAsRetired()
        {
            // Arrange
            var product = new Domain.Repositories.Models.Product(
                "product", "desc", 12.00, new Domain.Repositories.Models.Category(Guid.NewGuid(), "category", "desc"));
            this._productRepository.Get(product.Id).Returns(product);


            var category = new Category("category", "desc");
            this._categoryRepository.GetByProductId(product.Id)
                .Returns(category);

            // Act
            var retiredProduct = await this._productService.RetireProduct(product.Id);

            // Assert
            Assert.True(retiredProduct.Retired);
            Assert.Equal(product.Id, retiredProduct.Id);
            
            await this._categoryRepository.Received().GetByProductId(product.Id);
            await this._categoryRepository.Save(category);
            await this._productRepository.AddOrUpdate(product);
            
            
        }

        [Fact]
        public async void UpdateProduct_InvalidProductId_ReturnsNull()
        {
            // Arrange
            this._productRepository.Get(Arg.Any<Guid>()).Returns(null as Domain.Repositories.Models.Product);
            
            // Act / Assert
            Assert.Null(await this._productService.UpdateProduct(Guid.NewGuid(), new Product()));
        }

        [Fact]
        public async void UpdateProduct_InvalidProductNotAssociatedWithCategory_ThrowsException()
        {
            // Arrange
            var category = new Domain.Repositories.Models.Category(Guid.NewGuid(), "category", "desc");
            var product = new Domain.Repositories.Models.Product("product", "desc", 10.00, category);

            this._productRepository.Get(Arg.Any<Guid>()).Returns(product);
            this._categoryRepository.GetByProductId(product.Id).Returns(null as Category);

            // Act / Assert
            await Assert.ThrowsAsync<ProductServiceException>(() =>
                this._productService.UpdateProduct(product.Id, new Product()));
        }

        [Fact]
        public async void UpdateProduct_InvalidUpdatedCategory_ThrowsException()
        {
            // Arrange
            var oldCategory = new Category("category", "desc");
            var product = new Domain.Repositories.Models.Product(
                "product",
                "desc",
                10.00,
                new Domain.Repositories.Models.Category(oldCategory.Id, "category", "desc"));

            var updatedCategory = new Category("updated category", "desc");

            this._productRepository.Get(Arg.Any<Guid>()).Returns(product);
            this._categoryRepository.GetByProductId(product.Id).Returns(oldCategory);
            this._categoryRepository.Get(updatedCategory.Id).Returns(null as Category);

            var updatedProduct = new Product {Category = new CategoryId {Id = updatedCategory.Id}};
            
            // Act / Assert
            await Assert.ThrowsAsync<ProductServiceException>(() =>
                this._productService.UpdateProduct(product.Id, updatedProduct));
        }

        #region Setup

        private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IProductService _productService;

        #endregion
    }
}