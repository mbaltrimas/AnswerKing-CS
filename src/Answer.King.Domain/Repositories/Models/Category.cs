using System;

namespace Answer.King.Domain.Repositories.Models
{
    public class CategoryId
    {
        public CategoryId(Guid id)
        {
            Guard.AgainstDefaultValue(nameof(id), id);

            this.Id = id;
        }

        public Guid Id { get; }
    }
}