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

        [Fact]
        public async void CreateCategory_ValidCategory_ReturnsNewlyCreatedCategory()
        {
            // Arrange
            var request = new RequestModels.Category
            {
                Name = "category",
                Description = "desc"
            };

            // Act
            var category = await this._categoryService.CreateCategory(request);
            
            // Assert
            Assert.Equal(request.Name, category.Name);
            Assert.Equal(request.Description, category.Description);

            await this._categoryRepository.Received().Save(Arg.Any<Category>());
        }
        
        [Fact]
        public async void UpdateCategory_InvalidCategoryId_ReturnsNull()
        {
            // Arrange
            var updateCategoryRequest = new RequestModels.Category();
            var categoryId = Guid.NewGuid();
            
            // Act
            var category = await this._categoryService.UpdateCategory(categoryId, updateCategoryRequest);
            
            // Assert
            Assert.Null(category);
        }
        
        [Fact]
        public async void UpdateCategory_ValidCategoryIdAndRequest_ReturnsUpdatedCategory()
        {
            // Arrange
            var oldCategory = new Category("old category", "old desc");
            var categoryId = oldCategory.Id;
            
            var updateCategoryRequest = new RequestModels.Category
            {
                Name = "updated category",
                Description = "updated desc"
            };

            this._categoryRepository.Get(categoryId).Returns(oldCategory);
            
            // Act
            var actualCategory = await this._categoryService.UpdateCategory(categoryId, updateCategoryRequest);
            
            // Assert
            Assert.Equal(updateCategoryRequest.Name, actualCategory.Name);
            Assert.Equal(updateCategoryRequest.Description, actualCategory.Description);

            await this._categoryRepository.Received().Get(categoryId);
            await this._categoryRepository.Received().Save(Arg.Any<Category>());
        }
        
        #region Setup
        
        private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly CategoryService _categoryService;
        
        #endregion
    }
}