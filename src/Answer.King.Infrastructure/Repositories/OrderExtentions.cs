using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Answer.King.Domain.Orders;
using Answer.King.Domain.Orders.Models;

namespace Answer.King.Infrastructure.Repositories
{
    internal static class OrderFactory
    {
        public static Order CreateOrder(
            Guid id,
            DateTime createdOn,
            DateTime lastUpdated,
            OrderStatus status,
            IList<LineItem> lineItems)
        {
            var ctor = typeof(Order)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .SingleOrDefault(c => c.IsPrivate);

            var parameters = new object[] {id, createdOn, lastUpdated, status, lineItems};

            return (Order)ctor?.Invoke(parameters);
        }
    }
}