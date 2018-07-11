using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Domain.Orders;

namespace Answer.King.Api.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(RequestModels.Order createOrder);

        Task<Order> GetOrder(Guid orderId);

        Task<IEnumerable<Order>> GetOrders();

        Task<Order> UpdateOrder(Guid orderId, RequestModels.Order updateOrder);

        Task<Order> CancelOrder(Guid orderId);
    }
}