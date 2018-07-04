using System;

namespace Answer.King.Domain.Orders.Models
{
    public class Product
    {
        public Product(Guid id, double price)
        {
            Guard.AgainstDefaultValue(nameof(id), id);
            Guard.AgainstNegativeValue(nameof(price), price);

            this.Id = id;
            this.Price = price;
        }

        public Guid Id { get; }

        public double Price { get; }
    }
}