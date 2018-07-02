using Answer.King.Domain.Repositories.Models;
using Answer.King.Test.Common.CustomTraits;
using System;
using Xunit;

namespace Answer.King.Domain.UnitTests.Repositories.Models
{
    [TestCategory(TestType.Unit)]
    public class ProductTests
    {
        [Fact]
        public void Product_InitWithDefaultGuid_ThrowsDefaultValueException()
        {
            // Arrange
            var id = default(Guid);
            var productName = "Product Name";
            var productDescription = "Product Description";
            var category = this.GetCategory();
            var price = 142;

            // Act / Assert

            Assert.Throws<Guard.DefaultValueException>(() => new Product(
                id,
                productName,
                productDescription,
                price,
                category)
            );
        }

        [Fact]
        public void Product_InitWithNullName_ThrowsArgumentNullException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productName = (null as string);
            var productDescription = "Product Description";
            var category = this.GetCategory();
            var price = 142;

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Product(
                id,
                productName,
                productDescription,
                price,
                category)
            );
        }

        [Fact]
        public void Product_InitWithEmptyStringName_ThrowsEmptyStringException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productName = "";
            var productDescription = "Product Description";
            var category = this.GetCategory();
            var price = 142;

            // Act / Assert
            Assert.Throws<Guard.EmptyStringException>(() => new Product(
                id,
                productName,
                productDescription,
                price,
                category)
           );
        }

        [Fact]
        public void Product_InitWithNullDescription_ThrowsArgumentNullException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productName = "Product Name";
            var productDescription = (null as string);
            var category = this.GetCategory();
            var price = 142;

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Product(
                id,
                productName,
                productDescription,
                price,
                category)
            );
        }

        [Fact]
        public void Product_InitWithEmptyStringDescription_ThrowsEmptyStringException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productName = "Product Name";
            var productDescription = "";
            var category = this.GetCategory();
            var price = 142;

            // Act / Assert
            Assert.Throws<Guard.EmptyStringException>(() => new Product(
                id,
                productName,
                productDescription,
                price,
                category)
            );
        }

        [Fact]
        public void Product_InitWithNullCategory_ThrowsNullArgumentException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productName = "Product Name";
            var productDescription = "Product Description";
            var category = (null as Category);
            var price = 142;

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Product(
                id,
                productName,
                productDescription,
                price,
                category)
            );
        }

        [Fact]
        public void Product_InitWithNegativePrice_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productName = "Product Name";
            var productDescription = "Product Description";
            var category = this.GetCategory();
            var price = -1;

            // Act Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Product(
                id,
                productName,
                productDescription,
                price,
                category)
            );
        }

        #region Helpers

        private Category GetCategory() => new Category(
            Guid.NewGuid(),
            "Category Name",
            "Category Description"
        );

        #endregion Helpers
    }
}