using System;

namespace Answer.King.Domain.Repositories.Models
{
    public class Payment
    {
        public Payment(Guid orderId, double amount)
        {
            Guard.AgainstDefaultValue(nameof(orderId), orderId);
            Guard.AgainstNegativeValue(nameof(amount), amount);

            this.Id = Guid.NewGuid();
            this.OrderId = orderId;
            this.Amount = amount;
            this.Date = DateTime.UtcNow;
        }

        private Payment(Guid id, Guid orderId, double amount, DateTime date)
        {
            Guard.AgainstDefaultValue(nameof(id), id);
            Guard.AgainstDefaultValue(nameof(orderId), orderId);
            Guard.AgainstNegativeValue(nameof(amount), amount);
            Guard.AgainstDefaultValue(nameof(date), date);

            this.Id = id;
            this.OrderId = orderId;
            this.Amount = amount;
            this.Date = date;
        }

        public Guid Id { get; }

        public Guid OrderId { get; }

        public double Amount { get; }

        public DateTime Date { get; }
    }
}
