using System;
using System.Collections.Generic;
using System.Linq;
using Answer.King.Domain.Orders;
using Answer.King.Domain.Orders.Models;
using Product = Answer.King.Domain.Repositories.Models.Product;

namespace Answer.King.Api.Services.Extensions
{
    public static class OrderExtensions
    {
        public static void AddOrRemoveLineItems(this Order order, RequestModels.Order createOrder, IList<Product> domainProducts)
        {
            var newProductIds = createOrder.LineItems.Select(li => li.Product.Id);

            var lineItemsToRemove =
                order.LineItems
                    .Where(x => !newProductIds.Contains(x.Product.Id))
                    .ToList();

            lineItemsToRemove.ForEach(li => order.RemoveLineItem(li.Product.Id, li.Quantity));

            var addRemoveActions =
                order.LineItems.Select(lineItem =>
                {
                    var updatedLineItem =
                        createOrder.LineItems
                            .First(li => li.Product.Id == lineItem.Product.Id);

                    return new
                    {
                        ProductId = lineItem.Product.Id,
                        QuantityDifference = Math.Abs(lineItem.Quantity - updatedLineItem.Quantity),
                        IsIncrease = lineItem.Quantity < updatedLineItem.Quantity
                    };
                });

            foreach (var action in addRemoveActions)
            {
                if (action.IsIncrease)
                {
                    var product = domainProducts.Single(p => p.Id == action.ProductId);
                    var category = new Category(product.Category.Id, product.Category.Name, product.Category.Description);

                    order.AddLineItem(
                        product.Id,
                        product.Name,
                        product.Description,
                        product.Price,
                        category,
                        action.QuantityDifference);
                }
                else
                {
                    order.RemoveLineItem(action.ProductId, action.QuantityDifference);
                }
            }

            var oldProductIds = order.LineItems.Select(li => li.Product.Id);

            var lineItemsToAdd =
                createOrder.LineItems
                    .Where(x => !oldProductIds.Contains(x.Product.Id))
                    .ToList();

            lineItemsToAdd.ForEach(li =>
            {
                var product = domainProducts.Single(p => p.Id == li.Product.Id);
                var category = new Category(product.Category.Id, product.Category.Name, product.Category.Description);

                order.AddLineItem(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    category,
                    li.Quantity);
            });
        }
    }
}
