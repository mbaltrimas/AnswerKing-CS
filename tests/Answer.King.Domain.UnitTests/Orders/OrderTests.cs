using Answer.King.Domain.Orders;
using Answer.King.Test.Common.CustomTraits;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Answer.King.Domain.Orders.Models;
using Xunit;

namespace Answer.King.Domain.UnitTests.Orders
{
    [TestCategory(TestType.Unit)]
    public class OrderTests
    {
        [Fact]
        public void OrderStateStateEnum_MapsToCorrectInt()
        {
            var totalStreamNamesTested = new OrderStateConstantsData().Count();
            var totalConstants = GetAll().Count();

            Assert.Equal(totalStreamNamesTested, totalConstants);
        }

        #region AddLineItem

        [Fact]
        public void AddLineItem_OrderStatusCompleted_ThrowsOrderLifecycleException()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var category = new Category(Guid.NewGuid(), "name", "description");
            var price = 1.24;
            var quantity = 2;

            order.CompleteOrder();

            // Act / Assert
            Assert.Throws<OrderLifeCycleException>(() =>
                order.AddLineItem(id, name, description, price, category, quantity));
        }

        [Fact]
        public void AddLineItem_OrderStatusCancelled_ThrowsOrderLifecycleException()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var category = new Category(Guid.NewGuid(), "name", "description");
            var price = 1.24;
            var quantity = 2;

            order.CancelOrder();

            // Act / Assert
            Assert.Throws<OrderLifeCycleException>(() =>
                order.AddLineItem(id, name, description, price, category, quantity));
        }

        [Fact]
        public void AddLineItem_ValidArgumentsWithNewItem_AddsToLineItemsWithCorrectQuantity()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var category = new Category(Guid.NewGuid(), "name", "description");
            var price = 1.24;
            var quantity = 2;

            // Act
            order.AddLineItem(id, name, description, price, category, quantity);

            var lineItem = order.LineItems.FirstOrDefault();

            // Assert
            Assert.NotNull(lineItem);
            Assert.Equal(lineItem.Quantity, quantity);
            Assert.NotNull(lineItem.Product);
            Assert.Equal(lineItem.Product.Id, id);
            Assert.Equal(lineItem.Product.Price, price);
        }

        [Fact]
        public void AddLineItem_ValidArgumentsWithNewItem_AddsToLineItemsAndCalculatesTheCorrectSubtotal()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var category = new Category(Guid.NewGuid(), "name", "description");
            var price = 1.24;
            var quantity = 2;

            // Act
            order.AddLineItem(id, name, description, price, category, quantity);

            var lineItem = order.LineItems.FirstOrDefault();

            // Assert
            Assert.NotNull(lineItem);
            Assert.Equal(lineItem.SubTotal, quantity * price);
        }

        [Fact]
        public void AddLineItem_ValidArgumentsWithNewItem_AddsToLineItemsWithCorrectPrice()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var name = "name";
            var description = "description";
            var category = new Category(Guid.NewGuid(), "name", "description");
            var price = 1.24;
            var quantity = 2;

            // Act
            order.AddLineItem(id, name, description, price, category, quantity);

            var lineItem = order.LineItems.FirstOrDefault();

            // Assert
            Assert.NotNull(lineItem);
            Assert.NotNull(lineItem.Product);
            Assert.Equal(lineItem.Product.Price, price);
        }

        #endregion AddLineItem

        #region RemoveLineItem

        [Fact]
        public void RemoveLineItem_OrderStatusCompleted_ThrowsOrderLifecycleException()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var quantity = 2;

            order.CompleteOrder();

            // Act / Assert
            Assert.Throws<OrderLifeCycleException>(() =>
                order.RemoveLineItem(id, quantity));
        }

        [Fact]
        public void RemoveLineItem_OrderStatusCancel_ThrowsOrderLifecycleException()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var quantity = 2;

            order.CancelOrder();

            // Act / Assert
            Assert.Throws<OrderLifeCycleException>(() =>
                order.RemoveLineItem(id, quantity));
        }

        [Fact]
        public void RemoveLineItem_LineItemDoesNotExistInOrder_DoesNotAttemptToRemoveFromOrder()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var quantity = 3;

            // Act
            order.RemoveLineItem(id, quantity);

            // Assert
            Assert.Empty(order.LineItems);
        }

        [Fact]
        public void RemoveLineItem_LineItemExistsInOrder_DecrementCorrectQuantityValue()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var price = 1.25;

            order.AddLineItem(id, price, 5);

            // Act
            order.RemoveLineItem(id, 3);

            var lineItem = order.LineItems.FirstOrDefault();

            // Assert
            Assert.NotNull(lineItem);
            Assert.Equal(2, lineItem.Quantity);
        }

        [Fact]
        public void RemoveLineItem_LineItemExistsInOrder_RemovedFromOrderIfQuantityGteCurrent()
        {
            // Arrange
            var order = new Order();
            var id = Guid.NewGuid();
            var price = 1.25;

            order.AddLineItem(id, price, 3);

            // Act
            order.RemoveLineItem(id, 3);

            var lineItem = order.LineItems.FirstOrDefault();

            // Assert
            Assert.Null(lineItem);
            Assert.Equal(0, order.LineItems.Count);
        }

        #endregion RemoveLineItem

        #region Helpers

        private static IEnumerable<string> GetAll()
        {
            var enumValues = Enum.GetNames(typeof(OrderStatus));

            return enumValues;
        }

        #endregion Helpers
    }

    #region ClassData

    public class OrderStateConstantsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            return new List<object[]>
            {
                new object[] { 0, OrderStatus.Created },
                new object[] { 1, OrderStatus.Paid },
                new object[] { 2, OrderStatus.Cancelled },
            }.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    #endregion ClassData
}