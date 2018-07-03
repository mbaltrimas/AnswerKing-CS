using System;
using System.Collections.Generic;
using System.Linq;
using Answer.King.Domain.Orders;

namespace Answer.King.Infrastructure.Repositories
{
    internal static class OrderData
    {
        public static IList<Order> Orders { get; } = GetOrders();

        private static IList<Order> GetOrders()
        {
            return new List<Order>
            {
                new Order(),
                OrderWithLineItems(),
                CancelledOrder()
            };
        }

        private static Order OrderWithLineItems()
        {
            var fish = ProductData.Products.Single(p => p.Id == Guid.Parse("8d9142c2-96a0-4808-b00a-c43aee40293f"));

            var lineItem1 = new LineItem(fish);
            lineItem1.AddQuantity(1);

            var chips = ProductData.Products.Single(p => p.Id == Guid.Parse("89828e46-6cff-438f-be1a-6fa9355cfe24"));

            var lineItem2 = new LineItem(chips);
            lineItem1.AddQuantity(2);

            var lineItems = new List<LineItem>
            {
                lineItem1,
                lineItem2
            };

            return OrderFactory.CreateOrder(
                Guid.Parse("a62acdf2-aeb8-4d6e-8dc9-b237d76df388"),
                DateTime.UtcNow.AddHours(-1),
                DateTime.UtcNow.AddMinutes(-10),
                OrderStatus.Created,
                lineItems
            );
        }

        private static Order CancelledOrder()
        {
            var lineItems = new List<LineItem>();

            return OrderFactory.CreateOrder(
                Guid.Parse("91e0ddf2-59e1-40ec-942b-7eb1b7c10ad3"),
                DateTime.UtcNow.AddHours(-3),
                DateTime.UtcNow.AddMinutes(-50),
                OrderStatus.Cancelled,
                lineItems
            );
        }
    }
}