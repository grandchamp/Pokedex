using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Pokedex.Domain.Adapters;
using Pokedex.PokeApiClient;
using Pokedex.PokeApiClient.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.Infrastructure.Adapters
{
    public class PokeApiPokemonDataAdapter : IPokemonDataAdapter
    {
        private readonly IPokeApiClient _client;
        private readonly ILogger<PokeApiPokemonDataAdapter> _logger;
        public PokeApiPokemonDataAdapter(IPokeApiClient client, ILogger<PokeApiPokemonDataAdapter> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Result<Domain.Entities.Pokemon>> GetPokemonByNameAsync(string name)
        {
            try
            {
                var pokemon = await _client.GetPokemonByNameAsync(name);
                var species = await _client.RequestByNamedApiResource<Species>(pokemon.Species);
                var habitat = await _client.RequestByNamedApiResource<Habitat>(species.Habitat);

                var domainPokemon = new Domain.Entities.Pokemon(pokemon.Name,
                                                                species.FlavorTextEntries.First(flavor => flavor.Language.Name.Equals("en")).FlavorText,
                                                                habitat.Name,
                                                                species.IsLegendary);

                return Result.Success(domainPokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error while fetching the pokemon {name} from PokeAPI.");

                return Result.Failure<Domain.Entities.Pokemon>("Error while fetching the pokemon");
            }
        }
    }
}
