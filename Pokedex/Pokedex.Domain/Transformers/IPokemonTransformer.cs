using Pokedex.Domain.Entities;
using System.Threading.Tasks;

namespace Pokedex.Domain.Transformers
{
    public interface IPokemonTransformer
    {
        PokemonTransformationActions TransformationType { get; }
        bool IsValidForTransformation(Pokemon pokemon);
        public Task<Pokemon> Transform(Pokemon pokemon);
    }
}
