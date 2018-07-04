using System;
using System.Collections.Generic;
using System.Text;

namespace Answer.King.Domain.Inventory.Models
{
    public class Product
    {
        public Product(Guid id)
        {
            Guard.AgainstDefaultValue(nameof(id), id);

            this.Id = id;
        }

        public Guid Id { get; }
    }
}
