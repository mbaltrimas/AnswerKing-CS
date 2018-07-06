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
                serialize: cat =>
                {
                    var doc = new BsonDocument
                    {
                        ["_id"] = cat.Id,
                        ["Name"] = cat.Name,
                        ["Description"] = cat.Description,
                        ["Price"] = cat.Price,
                        ["Category"] = new BsonDocument { ["_id"] = cat.Category.Id },
                        ["Retired"] = cat.Retired
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