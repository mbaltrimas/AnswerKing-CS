using System;
using System.Collections.Generic;
using System.Linq;
using Answer.King.Infrastructure.Repositories.Mappings;

namespace Answer.King.Infrastructure.Extensions.DependancyInjection
{
    public class LiteDbEntityMappingOptions
    {
        internal List<Type> TypesToRegister { get; } = new List<Type>();

        public void RegisterEntityMappingsFromAssemblyContaining<T>()
        {
            var assembly = typeof(T).Assembly;
            var types = assembly.GetTypes()
                .Where(t =>
                    typeof(IEntityMapping).IsAssignableFrom(t)
                    && !t.IsAbstract
                    && !t.IsInterface).ToList();

            this.TypesToRegister.AddRange(types);
        }
    }
}