using System.Collections.Generic;

namespace Pokedex.FunTranslationsClient.Configuration
{
    public class FunTranslationsClientConfiguration
    {
        public static string FunTranslationsClientConfigurationSection = "FunTranslations";
        public string BaseUrl { get; set; }
        public Dictionary<string, string> TranslationTypeEndpoints { get; set; }
    }
}
