using System;

namespace Pokedex.Domain.Entities
{
    [Flags]
    public enum PokemonTransformationActions
    {
        None = 0,
        TranslateDescription = 1
    }
}
