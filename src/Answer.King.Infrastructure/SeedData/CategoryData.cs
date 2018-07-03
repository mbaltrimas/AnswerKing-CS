using System;
using System.Collections.Generic;
using Answer.King.Domain.Repositories.Models;

namespace Answer.King.Infrastructure.SeedData
{
    public class CategoryData
    {
        public static IList<Category> Categories { get; } = new List<Category>
        {
                new Category(
                    Guid.Parse("0002015c-1997-4732-a2de-e526323e6146"),
                    "Seafood",
                    "Food from the oceans"),
                new Category(
                    Guid.Parse("e32701f8-d644-4d5d-bd52-2a31fdaba3df"),
                    "Saundries",
                    "Things that go with things.")
        };
    }
}
