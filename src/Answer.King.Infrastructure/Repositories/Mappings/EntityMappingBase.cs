using LiteDB;

namespace Answer.King.Infrastructure.Repositories.Mappings
{
    public abstract class EntityMappingBase : IEntityMapping
    {
        protected EntityMappingBase() => this.RegisterMapping(BsonMapper.Global);

        public abstract void RegisterMapping(BsonMapper mapper);
    }
}