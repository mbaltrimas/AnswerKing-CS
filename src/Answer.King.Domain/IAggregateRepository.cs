using System;
using System.Threading.Tasks;

namespace Answer.King.Domain
{
    public interface IAggregateRepository<T> where T : IAggregateRoot
    {
        Task<T> GetOrCreate(Guid id);

        Task Save(T aggregate);
    }
}