using Pokedex.FunTranslationsClient.Models;
using System.Threading.Tasks;

namespace Pokedex.FunTranslationsClient
{
    public interface IFunTranslationsClient
    {
        Task<string> TranslateText(string text, TranslationType translationType);
    }
}
