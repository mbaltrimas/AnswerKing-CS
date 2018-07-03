using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Answer.King.Domain
{
    public interface IAggregateRepository<T> where T : IAggregateRoot
    {
        Task<IEnumerable<T>> Get();

        Task<T> Get(Guid id);

        Task Save(T item);
    }
}