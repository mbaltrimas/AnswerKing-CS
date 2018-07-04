using System;
using System.Collections.Generic;
using System.Linq;
using Answer.King.Domain.Inventory.Models;

namespace Answer.King.Domain.Inventory
{
    // Todo: look at custom deserialisation: https://stackoverflow.com/questions/42336751/custom-deserialization
    public class Category : IAggregateRoot
    {
        public Category(string name, string description)
        {
            Guard.AgainstNullOrEmptyArgument(nameof(name), name);
            Guard.AgainstNullOrEmptyArgument(nameof(description), description);

            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Description = description;
            this.LastUpdated = this.CreatedOn = DateTime.UtcNow;
            this._Products = new List<Product>();
            this.Retired = false;
        }

        // ReSharper disable once UnusedMember.Local
        private Category(
            Guid id,
            string name,
            string description,
            DateTime createdOn,
            DateTime lastUpdated,
            IList<Product> products,
            bool retired)
        {
            Guard.AgainstDefaultValue(nameof(id), id);
            Guard.AgainstNullOrEmptyArgument(nameof(name), name);
            Guard.AgainstNullOrEmptyArgument(nameof(description), description);
            Guard.AgainstDefaultValue(nameof(createdOn), createdOn);
            Guard.AgainstDefaultValue(nameof(lastUpdated), lastUpdated);

            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.CreatedOn = createdOn;
            this.LastUpdated = lastUpdated;
            this._Products = products ?? new List<Product>();
            this.Retired = retired;
        }

        public Guid Id { get; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public DateTime CreatedOn { get; }

        public DateTime LastUpdated { get; private set; }

        private IList<Product> _Products { get; }

        public IReadOnlyCollection<Product> Products => this._Products as List<Product>;

        public bool Retired { get; private set; }

        public void Rename(string name, string description)
        {
            Guard.AgainstNullOrEmptyArgument(nameof(name), name);
            Guard.AgainstNullOrEmptyArgument(nameof(description), description);

            this.Name = name;
            this.Description = description;
            this.LastUpdated = DateTime.UtcNow;
        }

        public void AddProduct(Product product)
        {
            if (this.Retired)
            {
                throw new CategoryLifecycleException("Cannot add product to retired catgory.");
            }

            var exists = this._Products.Any(p => p.Id == product.Id);

            if (exists)
            {
                return;
            }

            this._Products.Add(product);

            this.LastUpdated = DateTime.UtcNow;
        }

        public void RemoveProduct(Product product)
        {
            if (this.Retired)
            {
                throw new CategoryLifecycleException("Cannot remove product from retired catgory.");
            }

            var existing = this._Products.SingleOrDefault(p => p.Id == product.Id);

            if (existing == null)
            {
                return;
            }

            this._Products.Remove(existing);

            this.LastUpdated = DateTime.UtcNow;
        }

        public void RetireCategory()
        {
            if (this.Retired)
            {
                return;
            }

            if (this._Products.Any())
            {
                throw new CategoryLifecycleException("Cannot retire category whilst there are still products assigned.");
            }

            this.Retired = true;

            this.LastUpdated = DateTime.UtcNow;
        }
    }

    [Serializable]
    public class CategoryLifecycleException : Exception
    {
        public CategoryLifecycleException(string message) : base(message)
        {
        }
    }
}
