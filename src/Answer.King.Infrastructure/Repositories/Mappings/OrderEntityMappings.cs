using System;
using System.Linq;
using Answer.King.Domain.Orders;
using Answer.King.Domain.Orders.Models;
using LiteDB;

namespace Answer.King.Infrastructure.Repositories.Mappings
{
    public class OrderEntityMappings : EntityMappingBase
    {
        public override void RegisterMapping(BsonMapper mapper)
        {
            mapper.RegisterType
            (
                serialize: order =>
                {

                    var lineItems = order.LineItems.Select(li => new BsonDocument
                    {
                        ["Product"] = new BsonDocument
                        {
                            ["_id"] = li.Product.Id,
                            ["Price"] = li.Product.Price
                        },
                        ["Quantity"] = li.Quantity
                    });

                    var doc = new BsonDocument
                    {
                        ["_id"] = order.Id,
                        ["CreatedOn"] = order.CreatedOn,
                        ["LastUpdated"] = order.LastUpdated,
                        ["OrderStatus"] = $"{order.OrderStatus}",
                        ["LineItems"] = new BsonArray(lineItems)
                    };

                    return doc;
                },
                deserialize: bson =>
                {
                    var doc = bson.AsDocument;

                    var lineItems = 
                        doc["LineItems"].AsArray.Select(this.ToLineItem)
                            .ToList();

                    return OrderFactory.CreateOrder(
                        doc["_id"].AsGuid,
                        doc["CreatedOn"].AsDateTime,
                        doc["LastUpdated"].AsDateTime,
                        (OrderStatus)Enum.Parse(typeof(OrderStatus), doc["OrderStatus"]),
                        lineItems);
                }
            );
        }

        private LineItem ToLineItem(BsonValue item)
        {
            var lineItem = item.AsDocument;
            var product = lineItem["Product"].AsDocument;

            var result = new LineItem(
                new Product(
                    product["_id"].AsGuid,
                    product["Price"].AsDouble)
            );

            result.AddQuantity(lineItem["Quantity"].AsInt32);

            return result;
        }
    }
}