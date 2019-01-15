using System;
using Answer.King.Api.Services;
using Answer.King.Domain.Inventory;
using Answer.King.Domain.Inventory.Models;
using Answer.King.Domain.Repositories;
using NSubstitute;
using Xunit;

namespace Answer.King.Api.UnitTests.Services
{
    public class CategoryServiceTests
    {   
        public CategoryServiceTests()
        {
            this._categoryService = new CategoryService(this._categoryRepository, this._productRepository);
        }

        [Fact]
        public async void RetireCategory_ReturnsNullIfCategoryNotFound()
        {
            // Arrange
            this._categoryRepository.Get(Arg.Any<Guid>()).Returns(null as Category);
          
            // Act / Assert
            Assert.Null(await this._categoryService.RetireCategory(Guid.NewGuid()));
        }

        [Fact]
        public async void RetireCategory_ThrowsExceptionIfProductsAreStillAssociatedWithCategory()
        {
            // Arrange
            var category = new Category("category", "desc");
            category.AddProduct(new ProductId(Guid.NewGuid()));

            this._categoryRepository.Get(category.Id).Returns(category);
            
            // Act / Assert
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                this._categoryService.RetireCategory(category.Id));
        }
        
        [Fact]
        public async void RetireCategory_CategoryIsSavedAsRetired()
        {
            // Arrange
            var category = new Category("category", "desc");
            this._categoryRepository.Get(category.Id).Returns(category);
            
            // Act
            var retiredCategory = await this._categoryService.RetireCategory(category.Id);

            // Assert
            Assert.True(retiredCategory.Retired);
        }
        
        #region Setup
        
        private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly CategoryService _categoryService;
        
        #endregion
    }
}