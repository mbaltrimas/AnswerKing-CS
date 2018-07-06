using Answer.King.Domain.Repositories.Models;
using Answer.King.Test.Common.CustomTraits;
using System;
using Xunit;

namespace Answer.King.Domain.UnitTests.Repositories.Models
{
    [TestCategory(TestType.Unit)]
    public class CategoryTests
    {
        [Fact]
        public void Category_InitWithWithDefaultGuid_ThrowsDefaultValueException()
        {
            // Arrange
            var id = default(Guid);
            var categoryName = "Category Name";

            // Act / Assert
            Assert.Throws<Guard.DefaultValueException>(() => new Category(id, categoryName));
        }

        [Fact]
        public void Category_InitWithNullName_ThrowsArgumentNullException()
        {
            // Arrange
            var id = Guid.Parse("5E2B0450-6652-490F-93A0-1CA7C2B82B66");

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Category(id, null));
        }

        [Fact]
        public void Category_InitWithEmptyStringName_ThrowsEmptyStringException()
        {
            // Arrange
            var id = Guid.Parse("5E2B0450-6652-490F-93A0-1CA7C2B82B66");
            var categoryName = "";

            // Act / Assert
            Assert.Throws<Guard.EmptyStringException>(() => new Category(id, categoryName));
        }
    }
}