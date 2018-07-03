using System;
using System.Collections.Generic;
using Answer.King.Domain.Orders;

namespace Answer.King.Infrastructure.Repositories
{
    internal static class ProductData
    {
        public static IList<Product> Products { get; } = new List<Product>
        {
            new Product(
                Guid.Parse("8d9142c2-96a0-4808-b00a-c43aee40293f"),
                "Fish",
                "Delicious and satesfying.",
                new Category(
                    Guid.Parse("0002015c-1997-4732-a2de-e526323e6146"),
                    "Seafood",
                    "Food from the oceans"),
                5.99),
            new Product(
                Guid.Parse("89828e46-6cff-438f-be1a-6fa9355cfe24"),
                "Chips",
                "Nothing More to say.",
                new Category(
                    Guid.Parse("e32701f8-d644-4d5d-bd52-2a31fdaba3df"),
                    "Saundries",
                    "Things that go with things."),
                2.99)
        };
    }
}