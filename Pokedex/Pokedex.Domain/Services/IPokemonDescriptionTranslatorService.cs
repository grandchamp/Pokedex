using Pokedex.Domain.Entities;
using System.Threading.Tasks;

namespace Pokedex.Domain.Services
{
    public interface IPokemonDescriptionTranslatorService
    {
        bool IsValidForTranslation(Pokemon pokemon);
        Task<string> TranslateDescriptionAsync(string description);
    }
}
