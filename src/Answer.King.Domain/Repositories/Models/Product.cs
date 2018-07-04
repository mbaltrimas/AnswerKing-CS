using System;
using Answer.King.Domain.Orders;

namespace Answer.King.Domain.Repositories.Models
{
    public class Product
    {
        public Product(Guid id, string name, string description, double price, Category category)
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

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public Category Category { get; set; }
    }
}