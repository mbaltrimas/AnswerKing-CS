using Answer.King.Domain.Orders.Models;
using Answer.King.Test.Common.CustomTraits;
using System;
using Xunit;

namespace Answer.King.Domain.UnitTests.Orders.Models
{
    [TestCategory(TestType.Unit)]
    public class ProductTests
    {
        [Fact]
        public void Product_InitWithDefaultGuid_ThrowsDefaultValueException()
        {
            // Arrange
            var id = default(Guid);
            var price = 142;

            // Act / Assert

            Assert.Throws<Guard.DefaultValueException>(() => new Product(
                id,
                price)
            );
        }

        [Fact]
        public void Product_InitWithNegativePrice_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var price = -1;

            // Act Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Product(
                id,
                price)
            );
        }
    }
}