using Pokedex.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokedex.Infrastructure
{
    public static class PokemonTransformationActionsExtensions
    {
        public static IEnumerable<PokemonTransformationActions> GetAllActions(this PokemonTransformationActions actions)
        {
            if (actions == PokemonTransformationActions.None)
                return Enumerable.Empty<PokemonTransformationActions>();

            return Enum.GetValues<PokemonTransformationActions>()
                       .Where(action => action != PokemonTransformationActions.None && actions.HasFlag(action));
        }
    }
}
