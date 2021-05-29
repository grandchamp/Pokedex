using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Pokedex.Domain.Entities;
using Pokedex.Infrastructure.Adapters;
using Pokedex.PokeApiClient.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.IntegrationTests.Adapters
{
    public class PokeApiPokemonDataAdapterTests
    {
        private static PokeApiClient.PokeApiClient CreatePokeApiClient()
        {
            var options = Options.Create(new PokeApiClientConfiguration
            {
                BaseUrl = "https://pokeapi.co/api/v2"
            });

            var client = new HttpClient();

            return new PokeApiClient.PokeApiClient(client, options);
        }
        
        [Fact]
        public async Task GetPokemonByNameAsync_Success()
        {
            var pokeApiClient = CreatePokeApiClient();
            var expected = new Pokemon("mewtwo",
                                       "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.", 
                                       "rare",
                                       true);

            var adapter = new PokeApiPokemonDataAdapter(pokeApiClient, NullLogger<PokeApiPokemonDataAdapter>.Instance);

            var result = await adapter.GetPokemonByNameAsync(expected.Name);

            Assert.True(result.IsSuccess);
            Assert.Equal(expected.Name, result.Value.Name);
            Assert.Equal(expected.Description, result.Value.Description);
            Assert.Equal(expected.Habitat, result.Value.Habitat);
            Assert.Equal(expected.IsLegendary, result.Value.IsLegendary);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_NotFoundPokemon()
        {
            var pokeApiClient = CreatePokeApiClient();
         
            var adapter = new PokeApiPokemonDataAdapter(pokeApiClient, NullLogger<PokeApiPokemonDataAdapter>.Instance);

            var result = await adapter.GetPokemonByNameAsync("notfoundpokemon");

            Assert.True(result.IsFailure);
        }
    }
}
