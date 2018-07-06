using System;
using Microsoft.Extensions.DependencyInjection;

namespace Answer.King.Infrastructure.Extensions.DependancyInjection
{
    public static class LiteDbServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureEntityMapping(this IServiceCollection services, Action<LiteDbEntityMappingOptions> setupAction = null)
        {
            var action = setupAction ?? delegate { };
            var options = new LiteDbEntityMappingOptions();

            action(options);
            options.TypesToRegister.ForEach(type => Activator.CreateInstance(type));

            return services;
        }
    }
}
