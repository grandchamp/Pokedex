using Microsoft.Extensions.Configuration;
using Pokedex.FunTranslationsClient;
using Pokedex.FunTranslationsClient.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFunTranslationsClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IFunTranslationsClient, FunTranslationsClient>();

            services.Configure<FunTranslationsClientConfiguration>(
                configuration.GetSection(FunTranslationsClientConfiguration.FunTranslationsClientConfigurationSection));

            return services;
        }
    }
}
