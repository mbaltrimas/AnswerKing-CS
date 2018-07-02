using System;

namespace Answer.King.Domain.Orders
{
    public class Category
    {
        public Category(Guid id, string name, string description)
        {
            Guard.AgainstDefaultValue(nameof(id), id);
            Guard.AgainstNullOrEmptyArgument(nameof(name), name);
            Guard.AgainstNullOrEmptyArgument(nameof(description), description);

            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }
    }
}