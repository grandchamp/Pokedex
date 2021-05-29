using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Pokedex.Domain.Entities;
using Pokedex.FunTranslationsClient.Configuration;
using Pokedex.Infrastructure.Transformers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.IntegrationTests.Transformers
{
    public class ShakespeareDescriptionPokemonTransformerTests
    {
        private static FunTranslationsClient.FunTranslationsClient CreateFunTranslationsClient()
        {
            var options = Options.Create(new FunTranslationsClientConfiguration
            {
                BaseUrl = "https://api.funtranslations.com/translate",
                TranslationTypeEndpoints = new Dictionary<string, string>
                {
                    ["Yoda"] = "yoda",
                    ["Shakespeare"] = "shakespeare"
                }
            });

            var client = new HttpClient();

            return new FunTranslationsClient.FunTranslationsClient(client, options);
        }
        [Fact]
        public async Task Transform_TranslationSuccess()
        {
            const string EXPECTED_DESCRIPTION_TRANSLATED = "Mine description to beest did translate";

            var client = CreateFunTranslationsClient();
            var sut = new ShakespeareDescriptionPokemonTransformer(client,
                                                                   NullLogger<ShakespeareDescriptionPokemonTransformer>.Instance);

            var pokemon = new Pokemon("poke", "my description to be translated", "rare", false);
            var result = await sut.Transform(pokemon);

            Assert.Equal(EXPECTED_DESCRIPTION_TRANSLATED, result.Description);
            Assert.Equal(pokemon.Name, result.Name);
            Assert.Equal(pokemon.Habitat, result.Habitat);
            Assert.Equal(pokemon.IsLegendary, result.IsLegendary);
        }
    }
}
