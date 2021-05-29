using Pokedex.Domain.Entities;
using System.Threading.Tasks;

namespace Pokedex.Domain.Transformers
{
    public interface IPokemonTransformer
    {
        public Task<Pokemon> Transform(Pokemon pokemon);
    }
}
