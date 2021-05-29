using CSharpFunctionalExtensions;
using Pokedex.Domain.Entities;
using System.Threading.Tasks;

namespace Pokedex.Domain.Adapters
{
    public interface IPokemonDataAdapter
    {
        Task<Result<Pokemon>> GetPokemonByNameAsync(string name);
    }
}
