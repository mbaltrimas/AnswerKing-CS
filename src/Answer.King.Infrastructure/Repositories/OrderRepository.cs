using System;
using System.Threading.Tasks;
using Answer.King.Domain.Orders;
using Answer.King.Domain.Repositories;

namespace Answer.King.Infrastructure.Aggregate
{
    public class OrderRepository : IOrderRepository
    {
        public Task<Order> GetOrCreate(Guid id)
        {
            throw new System.NotImplementedException();
        }

        public Task Save(Order aggregate)
        {
            throw new System.NotImplementedException();
        }
    }
}