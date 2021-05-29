using Pokedex.PokeApiClient.Models;
using System.Threading.Tasks;

namespace Pokedex.PokeApiClient
{
    public interface IPokeApiClient
    {
        Task<Pokemon> GetPokemonByNameAsync(string name);
        Task<T> RequestByNamedApiResource<T>(NamedApiResource resource);
    }
}
