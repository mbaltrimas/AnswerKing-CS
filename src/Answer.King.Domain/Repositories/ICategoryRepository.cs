using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Domain.Repositories.Models;

namespace Answer.King.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> Get(Guid id);

        Task<IEnumerable<Category>> Get();
    }
}