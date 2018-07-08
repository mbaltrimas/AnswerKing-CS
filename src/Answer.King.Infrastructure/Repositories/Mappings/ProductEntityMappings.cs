using Answer.King.Domain.Repositories.Models;
using LiteDB;

namespace Answer.King.Infrastructure.Repositories.Mappings
{
    public class ProductEntityMappings : EntityMappingBase
    {
        public override void RegisterMapping(BsonMapper mapper)
        {
            mapper.RegisterType
            (
                serialize: product =>
                {
                    var doc = new BsonDocument
                    {
                        ["_id"] = product.Id,
                        ["Name"] = product.Name,
                        ["Description"] = product.Description,
                        ["Price"] = product.Price,
                        ["Category"] = new BsonDocument { ["_id"] = product.Category.Id },
                        ["Retired"] = product.Retired
                    };

                    return doc;
                },
                deserialize: bson =>
                {
                    var doc = bson.AsDocument;
                    var cat = doc["Category"].AsDocument;

                    return ProductFactory.CreateProduct(
                        doc["_id"].AsGuid,
                        doc["Name"].AsString,
                        doc["Description"].AsString,
                        doc["Price"].AsDouble,
                        new CategoryId(cat["_id"].AsGuid), 
                        doc["Retired"].AsBoolean);
                }
            );
        }
    }
}