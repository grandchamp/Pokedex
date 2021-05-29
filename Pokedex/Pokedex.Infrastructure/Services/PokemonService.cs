using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Pokedex.Domain.Adapters;
using Pokedex.Domain.Entities;
using Pokedex.Domain.Services;
using Pokedex.Domain.Transformers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.Infrastructure.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonDataAdapter _pokemonDataAdapter;
        private readonly Dictionary<PokemonTransformationActions, List<IPokemonTransformer>> _transformations;
        private readonly ILogger<PokemonService> _logger;
        public PokemonService(IPokemonDataAdapter pokemonDataAdapter,
            Dictionary<PokemonTransformationActions, List<IPokemonTransformer>> transformations,
            ILogger<PokemonService> logger)
        {
            _pokemonDataAdapter = pokemonDataAdapter;
            _transformations = transformations;
            _logger = logger;
        }

        public async Task<Result<Pokemon>> GetPokemonAsync(string name, PokemonTransformationActions transformationActions)
        {
            _logger.LogDebug($"Fetching pokemon data with adapter type {_pokemonDataAdapter.GetType()}");

            var pokemon = await _pokemonDataAdapter.GetPokemonByNameAsync(name);

            if (pokemon.IsFailure)
                return pokemon;

            pokemon = await ApplyTransformationsAsync(pokemon.Value, transformationActions.GetAllActions());

            return pokemon;
        }

        private async Task<Pokemon> ApplyTransformationsAsync(Pokemon pokemon, IEnumerable<PokemonTransformationActions> actions)
        {
            var modifiedPokemon = pokemon;

            foreach (var action in actions)
            {
                if (_transformations.TryGetValue(action, out var availableTransformations))
                {
                    foreach (var transformation in availableTransformations.Where(x => x.IsValidForTransformation(modifiedPokemon)))
                    {
                        modifiedPokemon = await transformation.Transform(modifiedPokemon);
                    }
                }
                else
                {
                    _logger.LogWarning($"No pokemon transformer was found for action {action}");

                }
            }

            return modifiedPokemon;
        }
    }
}
