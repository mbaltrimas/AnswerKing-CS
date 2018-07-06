using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Answer.King.Domain.Inventory;
using Answer.King.Domain.Inventory.Models;

namespace Answer.King.Infrastructure.Repositories.Mappings
{
    internal static class CategoryFactory
    {
        public static Category CreateOrder(
            Guid id,
            string name,
            string description,
            DateTime createdOn,
            DateTime lastUpdated,
            IList<ProductId> products,
            bool retired)
        {
            var ctor = typeof(Category)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .SingleOrDefault(c => c.IsPrivate && c.GetParameters().Any());

            var parameters = new object[]
            {
                id,
                name,
                description,
                createdOn,
                lastUpdated,
                products,
                retired
            };

            return (Category)ctor?.Invoke(parameters);
        }
    }
}