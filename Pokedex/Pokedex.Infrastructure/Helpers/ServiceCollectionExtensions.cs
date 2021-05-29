using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Domain.Adapters;
using Pokedex.Domain.Services;
using Pokedex.Domain.Transformers;
using Pokedex.Infrastructure.Adapters;
using Pokedex.Infrastructure.Services;
using Pokedex.Infrastructure.Transformers;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigurePokedexServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddAdapters()
                           .AddServices()
                           .AddTransformers()
                           .AddHttpClients(configuration);
        }

        private static IServiceCollection AddAdapters(this IServiceCollection services)
        {
            services.AddScoped<IPokemonDataAdapter, PokeApiPokemonDataAdapter>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPokemonService, PokemonService>();

            return services;
        }

        private static IServiceCollection AddTransformers(this IServiceCollection services)
        {
            services.AddScoped<IPokemonTransformer, ShakespeareDescriptionPokemonTransformer>();
            services.AddScoped<IPokemonTransformer, YodaDescriptionPokemonTransformer>();

            services.AddScoped((provider) =>
            {
                var transformers = provider.GetServices<IPokemonTransformer>();

                return transformers.GroupBy(transformer => transformer.TransformationType)
                                   .ToDictionary(group => group.Key, transformers => transformers.ToList());
            });

            return services;
        }

        private static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPokeApiClient(configuration);
            services.AddFunTranslationsClient(configuration);

            return services;
        }
    }
}
