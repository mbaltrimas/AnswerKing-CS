using System;

namespace Answer.King.Domain.Orders
{
    public class Product
    {
        public Product(Guid id, string name, string description, Category category, double price)
        {
            Guard.AgainstDefaultValue(nameof(id), id);
            Guard.AgainstNullOrEmptyArgument(nameof(name), name);
            Guard.AgainstNullOrEmptyArgument(nameof(description), description);
            Guard.AgainstNullArgument(nameof(category), category);
            Guard.AgainstNegativeValue(nameof(price), price);

            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Category = category;
            this.Price = price;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public double Price { get; }

        public Category Category { get; }
    }
}