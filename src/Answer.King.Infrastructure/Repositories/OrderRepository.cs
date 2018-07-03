using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Answer.King.Domain.Orders;
using Answer.King.Domain.Repositories;
using Answer.King.Infrastructure.SeedData;

namespace Answer.King.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository()
        {
            this.Orders = OrderData.Orders;
        }

        private IList<Order> Orders { get; }

        public Task<IEnumerable<Order>> Get()
        {
            return Task.FromResult(this.Orders as IEnumerable<Order>);
        }

        public Task<Order> Get(Guid id)
        {
            return Task.FromResult(this.Orders.SingleOrDefault(o => o.Id == id));
        }

        public Task Save(Order item)
        {
            this.Orders.Add(item);
            return Task.CompletedTask;
        }
    }
}