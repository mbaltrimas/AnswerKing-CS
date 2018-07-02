using Answer.King.Domain.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Answer.King.Domain.Orders
{
    public enum OrderStatus
    {
        Created = 0,
        Paid = 1,
        Cancelled = 2
    }

    // Todo: look at custom deserialisation: https://stackoverflow.com/questions/42336751/custom-deserialization
    public class Order : IAggregateRoot
    {
        public Order()
        {
            this.Id = Guid.NewGuid();
            this.LastUpdated = this.CreatedOn = DateTime.UtcNow;
            this.OrderStatus = OrderStatus.Created;
            this._LineItems = new List<LineItem>();
            this.Payments = new List<float>();
        }

        internal Order(
            Guid id,
            DateTime createdOn,
            DateTime lastUpdated,
            OrderStatus status,
            IList<LineItem> lineItems,
            IList<float> payments)
        {
            Guard.AgainstDefaultValue(nameof(id), id);
            Guard.AgainstDefaultValue(nameof(createdOn), createdOn);
            Guard.AgainstDefaultValue(nameof(lastUpdated), lastUpdated);

            this.Id = id;
            this.CreatedOn = createdOn;
            this.LastUpdated = lastUpdated;
            this.OrderStatus = status;
            this._LineItems = lineItems;
            this.Payments = payments;
        }

        public Guid Id { get; }

        public DateTime CreatedOn { get; }

        public DateTime LastUpdated { get; private set; }

        public OrderStatus OrderStatus { get; private set; }

        public float OrderTotal => this.LineItems.Sum(li => li.Product.Price);

        private float Balance => this.OrderTotal - this.Payments.Sum();

        private IList<float> Payments { get; }

        private IList<LineItem> _LineItems { get; }

        public IReadOnlyCollection<LineItem> LineItems => this._LineItems as List<LineItem>;

        public void AddLineItem(Guid productId, string name, string description, float price, Category category, int quantity = 1)
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

        public void MakePayment(float amount)
        {
            if (this.OrderStatus != OrderStatus.Created)
            {
                throw new OrderLifeCycleException($"Cannot make payment - Order status {this.OrderStatus}.");
            }

            if (amount > this.Balance)
            {
                throw new OrderPaymentException($"Balance £{this.Balance} is less than amount being paid £{amount}");
            }

            this.Payments.Add(amount);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (this.Balance == 0.0)
            {
                this.OrderStatus = OrderStatus.Paid;
            }

            this.LastUpdated = DateTime.UtcNow;
        }

        public void CancelOrder()
        {
            if (this.OrderStatus == OrderStatus.Cancelled)
            {
                throw new OrderLifeCycleException($"Cannot cancel - Order status {this.OrderStatus}.");
            }

            this.OrderStatus = OrderStatus.Cancelled;
            this.LastUpdated = DateTime.UtcNow;
        }
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
