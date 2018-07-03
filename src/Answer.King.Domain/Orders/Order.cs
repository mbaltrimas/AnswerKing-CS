using System;
using System.Collections.Generic;
using System.Linq;

namespace Answer.King.Domain.Orders
{
    // Todo: look at custom deserialisation: https://stackoverflow.com/questions/42336751/custom-deserialization
    public class Order : IAggregateRoot
    {
        public Order()
        {
            this.Id = Guid.NewGuid();
            this.LastUpdated = this.CreatedOn = DateTime.UtcNow;
            this.OrderStatus = OrderStatus.Created;
            this._LineItems = new List<LineItem>();
        }

        // ReSharper disable once UnusedMember.Local
        private Order(
            Guid id,
            DateTime createdOn,
            DateTime lastUpdated,
            OrderStatus status,
            IList<LineItem> lineItems)
        {
            Guard.AgainstDefaultValue(nameof(id), id);
            Guard.AgainstDefaultValue(nameof(createdOn), createdOn);
            Guard.AgainstDefaultValue(nameof(lastUpdated), lastUpdated);

            this.Id = id;
            this.CreatedOn = createdOn;
            this.LastUpdated = lastUpdated;
            this.OrderStatus = status;
            this._LineItems = lineItems;
        }

        public Guid Id { get; }

        public DateTime CreatedOn { get; }

        public DateTime LastUpdated { get; private set; }

        public OrderStatus OrderStatus { get; private set; }

        public double OrderTotal => this.LineItems.Sum(li => li.Product.Price);

        private IList<LineItem> _LineItems { get; }

        public IReadOnlyCollection<LineItem> LineItems => this._LineItems as List<LineItem>;

        public void AddLineItem(Guid productId, string name, string description, double price, Category category, int quantity = 1)
        {
            if (this.OrderStatus != OrderStatus.Created)
            {
                throw new OrderLifeCycleException($"Cannot add line item - Order status {this.OrderStatus}.");
            }

            var lineItem = this._LineItems.SingleOrDefault(li => li.Product.Id == productId);

            if (lineItem == null)
            {
                var product = new Product(productId, name, description, category, price);
                lineItem = new LineItem(product);
            }

            lineItem.AddQuantity(quantity);
            this.LastUpdated = DateTime.UtcNow;
        }

        public void RemoveLineItem(Guid productId, int quantity = 1)
        {
            if (this.OrderStatus != OrderStatus.Created)
            {
                throw new OrderLifeCycleException($"Cannot remove line item - Order status {this.OrderStatus}.");
            }

            var lineItem = this._LineItems.SingleOrDefault(li => li.Product.Id == productId);

            if (lineItem != null)
            {
                lineItem.RemoveQuantity(quantity);
                this.LastUpdated = DateTime.UtcNow;
            }
        }

        public void CompleteOrder()
        {
            if (this.OrderStatus != OrderStatus.Completed)
            {
                throw new OrderLifeCycleException($"Cannot complete order - Order status {this.OrderStatus}.");
            }

            this.OrderStatus = OrderStatus.Completed;
            this.LastUpdated = DateTime.UtcNow;
        }

        public void CancelOrder()
        {
            if (this.OrderStatus == OrderStatus.Cancelled)
            {
                throw new OrderLifeCycleException($"Cannot cancel order - Order status {this.OrderStatus}.");
            }

            this.OrderStatus = OrderStatus.Cancelled;
            this.LastUpdated = DateTime.UtcNow;
        }
    }

    public enum OrderStatus
    {
        Created = 0,
        Completed = 1,
        Cancelled = 2
    }

    [Serializable]
    public class OrderPaymentException : Exception
    {
        public OrderPaymentException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class OrderLifeCycleException : Exception
    {
        public OrderLifeCycleException(string message) : base(message)
        {
        }
    }
}
