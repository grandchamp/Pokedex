using CSharpFunctionalExtensions;
using Pokedex.Domain.Entities;
using System.Threading.Tasks;

namespace Pokedex.Domain.Services
{
    public interface IPokemonService
    {
        Task<Result<Pokemon>> GetPokemonAsync(string name, PokemonTransformationActions transformationActions);
    }
}
