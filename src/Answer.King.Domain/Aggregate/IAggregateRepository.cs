using System;
using System.Threading.Tasks;

namespace Answer.King.Domain.Aggregate
{
    public interface IAggregateRepository
    {
        Task<T> GetOrCreate<T>(Guid id) where T : IAggregateRoot;

        Task Save(IAggregateRoot aggregate);
    }
}