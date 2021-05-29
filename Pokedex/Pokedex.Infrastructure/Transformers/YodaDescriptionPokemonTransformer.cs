using Microsoft.Extensions.Logging;
using Pokedex.Domain.Entities;
using Pokedex.Domain.Transformers;
using Pokedex.FunTranslationsClient;
using Pokedex.FunTranslationsClient.Models;
using System;
using System.Threading.Tasks;

namespace Pokedex.Infrastructure.Transformers
{
    public class YodaDescriptionPokemonTransformer : IPokemonTransformer
    {
        public PokemonTransformationActions TransformationType => PokemonTransformationActions.TranslateDescription;

        private readonly IFunTranslationsClient _funTranslationsClient;
        private readonly ILogger<YodaDescriptionPokemonTransformer> _logger;
        public YodaDescriptionPokemonTransformer(IFunTranslationsClient funTranslationsClient, ILogger<YodaDescriptionPokemonTransformer> logger)
        {
            _funTranslationsClient = funTranslationsClient;
            _logger = logger;
        }

        public bool IsValidForTransformation(Pokemon pokemon)
            => pokemon.Habitat.Equals("cave", StringComparison.OrdinalIgnoreCase) || pokemon.IsLegendary;

        public async Task<Pokemon> Transform(Pokemon pokemon)
        {
            if (!IsValidForTransformation(pokemon))
                return pokemon;

            try
            {
                var translation = await _funTranslationsClient.TranslateText(pokemon.Description, TranslationType.Yoda);

                return new Pokemon(pokemon.Name, translation, pokemon.Habitat, pokemon.IsLegendary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yoda translation failed.");

                return pokemon;
            }
        }
    }
}
