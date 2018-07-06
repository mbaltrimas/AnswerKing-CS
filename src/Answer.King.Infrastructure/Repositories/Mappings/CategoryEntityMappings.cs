using System.Linq;
using Answer.King.Domain.Inventory.Models;
using LiteDB;

namespace Answer.King.Infrastructure.Repositories.Mappings
{
    public class CategoryEntityMappings : EntityMappingBase
    {
        public override void RegisterMapping(BsonMapper mapper)
        {
            mapper.RegisterType
            (
                serialize: cat =>
                {
                    var products = cat.Products.Select(p => new BsonDocument { ["_id"] = p.Id });

                    var doc = new BsonDocument
                    {
                        ["_id"] = cat.Id,
                        ["Name"] = cat.Name,
                        ["Description"] = cat.Description,
                        ["CreatedOn"] = cat.CreatedOn,
                        ["LastUpdated"] = cat.LastUpdated,
                        ["Products"] = new BsonArray(products),
                        ["Retired"] = cat.Retired
                    };

                    return doc;
                },
                deserialize: bson =>
                {
                    var doc = bson.AsDocument;

                    return CategoryFactory.CreateOrder(
                        doc["_id"].AsGuid,
                        doc["Name"].AsString,
                        doc["Description"].AsString,
                        doc["CreatedOn"].AsDateTime,
                        doc["LastUpdated"].AsDateTime,
                        doc["Products"].AsArray.Select(
                            p => new ProductId(p.AsDocument["_id"].AsGuid)).ToList(),
                        doc["Retired"].AsBoolean);
                }
            );
        }
    }
}
