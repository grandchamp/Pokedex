using Microsoft.Extensions.Logging;
using Pokedex.Domain.Entities;
using Pokedex.Domain.Transformers;
using Pokedex.FunTranslationsClient;
using Pokedex.FunTranslationsClient.Models;
using System;
using System.Threading.Tasks;

namespace Pokedex.Infrastructure.Transformers
{
    public class ShakespeareDescriptionPokemonTransformer : IPokemonTransformer
    {
        public PokemonTransformationActions TransformationType => PokemonTransformationActions.TranslateDescription;

        private readonly IFunTranslationsClient _funTranslationsClient;
        private readonly ILogger<ShakespeareDescriptionPokemonTransformer> _logger;
        public ShakespeareDescriptionPokemonTransformer(IFunTranslationsClient funTranslationsClient, ILogger<ShakespeareDescriptionPokemonTransformer> logger)
        {
            _funTranslationsClient = funTranslationsClient;
            _logger = logger;
        }

        public bool IsValidForTransformation(Pokemon pokemon)
            => !(pokemon.Habitat.Equals("cave", StringComparison.OrdinalIgnoreCase) || pokemon.IsLegendary);

        public async Task<Pokemon> Transform(Pokemon pokemon)
        {
            if (!IsValidForTransformation(pokemon))
                return pokemon;

            try
            {
                var translation = await _funTranslationsClient.TranslateText(pokemon.Description, TranslationType.Shakespeare);

                return new Pokemon(pokemon.Name, translation, pokemon.Habitat, pokemon.IsLegendary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Shakespeare translation failed.");

                return pokemon;
            }
        }
    }
}
