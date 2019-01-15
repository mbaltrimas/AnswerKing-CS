using System;
using Answer.King.Domain.Inventory;
using Answer.King.Domain.Inventory.Models;
using Answer.King.Test.Common.CustomTraits;
using Xunit;

namespace Answer.King.Domain.UnitTests.Inventory
{
    [TestCategory(TestType.Unit)]
    public class CategoryTests
    {
        [Fact]
        public void RenameCategory_WithValidNameAndDescription()
        {
            var category = new Category("Phones", "Electronics");

            category.Rename("Lemon", "Squash");
            
            Assert.Equal("Lemon", category.Name);
            Assert.Equal("Squash", category.Description);
        }

        [Fact]
        public void RenameCategory_InvalidNameThrowsExceptionWhenNull()
        {
            var category = new Category("Phones", "Electronics");
            Assert.Throws<ArgumentNullException>(() => category.Rename(null, "Electronics"));
        }
        
        [Fact]
        public void RenameCategory_InvalidNameThrowsExceptionWhenEmpty()
        {
            var category = new Category("Phones", "Electronics");
            Assert.Throws<Guard.EmptyStringException>(() => category.Rename("", "Electronics"));
        }
        
        [Fact]
        public void RenameCategory_InvalidDescriptionThrowsExceptionWhenNull()
        {
            var category = new Category("Phones", "Electronics");
            Assert.Throws<ArgumentNullException>(() => category.Rename("Phones", null));
        }
        
        [Fact]
        public void RenameCategory_InvalidDescriptionThrowsExceptionWhenEmpty()
        {
            var category = new Category("Phones", "Electronics");
            Assert.Throws<Guard.EmptyStringException>(() => category.Rename("Phones", ""));
        }

        [Fact]
        public void RetireCategory_ThrowsExceptionIfAnyProductAssociatedWithCategory()
        {
            var category = new Category("Phones", "Electronics");
            category.AddProduct(new ProductId(Guid.NewGuid()));

            Assert.Throws<CategoryLifecycleException>(() => category.RetireCategory());
        }
    }
}