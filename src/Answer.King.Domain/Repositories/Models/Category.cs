using System;

namespace Answer.King.Domain.Repositories.Models
{
    public class Category
    {
        public Category(Guid id, string name)
        {
            Guard.AgainstDefaultValue(nameof(id), id);
            Guard.AgainstNullOrEmptyArgument(nameof(name), name);

            this.Id = id;
            this.Name = name;
        }

        public Guid Id { get; }

        public string Name { get; }
    }
}