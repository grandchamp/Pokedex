using CSharpFunctionalExtensions;
using Pokedex.Domain.Entities;
using System.Threading.Tasks;

namespace Pokedex.Domain.Gateways
{
    public interface IPokemonService
    {
        Task<Result<Pokemon>> GetPokemonAsync(string name, PokemonTransformationActions transformationActions);
    }
}
