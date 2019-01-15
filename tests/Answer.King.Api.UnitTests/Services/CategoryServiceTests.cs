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
        private readonly ICategoryRepository CategoryRepository = Substitute.For<ICategoryRepository>();
        private readonly IProductRepository ProductRepository = Substitute.For<IProductRepository>();
        private readonly CategoryService CategoryService;

        public CategoryServiceTests()
        {
            this.CategoryService = new CategoryService(this.CategoryRepository, this.ProductRepository);
        }

        [Fact]
        public async void RetireCategory_ReturnsNullIfCategoryNotFound()
        {
            this.CategoryRepository.Get(Arg.Any<Guid>()).Returns(null as Category);
            Assert.Null(await this.CategoryService.GetCategory(Guid.NewGuid()));
        }

        [Fact]
        public async void RetireCategory_ThrowsExceptionIfProductsAreStillAssociatedWithCategory()
        {
            var category = new Category("category", "desc");
            category.AddProduct(new ProductId(Guid.NewGuid()));

            this.CategoryRepository.Get(category.Id).Returns(category);
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                this.CategoryService.RetireCategory(category.Id));
        }
        
        [Fact]
        public async void RetireCategory_CategoryIsSavedAsRetired()
        {
            var category = new Category("category", "desc");
            this.CategoryRepository.Get(category.Id).Returns(category);
            
            var retiredCategory = await this.CategoryService.RetireCategory(category.Id);

            Assert.True(retiredCategory.Retired);
        }
    }
}