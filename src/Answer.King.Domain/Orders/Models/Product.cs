using System;

namespace Answer.King.Domain.Orders.Models
{
    public class Product
    {
        public Product(Guid id, string name, double price)
        {
            Guard.AgainstDefaultValue(nameof(id), id);
            Guard.AgainstNullOrEmptyArgument(nameof(name), name);
            Guard.AgainstNegativeValue(nameof(price), price);

            this.Id = id;
            this.Name = name;
            this.Price = price;
        }

        public Guid Id { get; }

        public string Name { get; }

        public double Price { get; }
    }
}