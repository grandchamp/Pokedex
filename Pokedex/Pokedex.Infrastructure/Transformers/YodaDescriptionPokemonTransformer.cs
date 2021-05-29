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
        public YodaDescriptionPokemonTransformer(IFunTranslationsClient funTranslationsClient)
        {
            _funTranslationsClient = funTranslationsClient;
        }

        public bool IsValidForTransformation(Pokemon pokemon)
            => !(pokemon.Habitat.Equals("cave", StringComparison.OrdinalIgnoreCase) || pokemon.IsLegendary);

        public async Task<Pokemon> Transform(Pokemon pokemon)
        {
            var translation = await _funTranslationsClient.TranslateText(pokemon.Description, TranslationType.Yoda);

            return new Pokemon(pokemon.Name, translation, pokemon.Habitat, pokemon.IsLegendary);
        }
    }
}
