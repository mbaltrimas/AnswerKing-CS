using System;
using System.Threading.Tasks;

namespace Answer.King.Domain
{
    public interface IAggregateRepository<T> where T : IAggregateRoot
    {
        Task<T> Get(Guid id);

        Task Save(T item);
    }
}