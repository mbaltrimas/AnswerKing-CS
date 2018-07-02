using System;
using System.Threading.Tasks;
using Answer.King.Domain.Aggregate;

namespace Answer.King.Infrastructure.Aggregate
{
    public class AggregateRepository : IAggregateRepository
    {
        public Task<T> GetOrCreate<T>(Guid id) where T : IAggregateRoot
        {
            throw new System.NotImplementedException();
        }

        public Task Save(IAggregateRoot aggregate)
        {
            throw new System.NotImplementedException();
        }
    }
}