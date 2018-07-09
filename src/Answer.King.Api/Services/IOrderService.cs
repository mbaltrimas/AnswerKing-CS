using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Api.RequestModels;
using Answer.King.Domain.Orders;

namespace Answer.King.Api.Services
{
    public interface IOrderService
    {
        Task<Domain.Orders.Order> CreateOrder(RequestModels.Order createOrder);

        Task<Domain.Orders.Order> GetOrder(Guid orderId);

        Task<IEnumerable<Domain.Orders.Order>> GetOrders();

        Task<Domain.Orders.Order> UpdateOrder(Guid orderId, RequestModels.Order updateOrder);

        Task<Domain.Orders.Order> CancelOrder(Guid orderId);
    }
}