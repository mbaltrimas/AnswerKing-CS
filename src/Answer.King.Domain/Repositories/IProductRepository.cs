using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Domain.Orders;

namespace Answer.King.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product> Get(Guid id);

        Task<IEnumerable<Product>> Get();
    }
}
