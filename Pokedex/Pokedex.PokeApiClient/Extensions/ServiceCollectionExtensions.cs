using Microsoft.Extensions.Configuration;
using Pokedex.PokeApiClient;
using Pokedex.PokeApiClient.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPokeApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IPokeApiClient, PokeApiClient>();

            services.Configure<PokeApiClientConfiguration>(configuration.GetSection(PokeApiClientConfiguration.PokeApiConfigurationSection));

            return services;
        }
    }
}
