using System.Collections.Generic;
using System.Linq;
using Answer.King.Domain.Orders;
using Answer.King.Domain.Orders.Models;
using Product = Answer.King.Domain.Repositories.Models.Product;

namespace Answer.King.Api.Services.Extensions
{
    public static class OrderExtensions
    {
        public static void AddLineItems(this Order order, RequestModels.Order createOrder, IList<Product> domainProducts)
        {
            foreach (var lineItem in createOrder.LineItems)
            {
                var product = domainProducts.Single(p => p.Id == lineItem.Product.Id);
                var category = new Category(product.Category.Id, product.Category.Name, product.Category.Description);

                order.AddLineItem(product.Id, product.Name, product.Description, product.Price, category, lineItem.Quantity);
            }
        }
    }
}
