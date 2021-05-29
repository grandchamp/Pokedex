using System.Text.Json.Serialization;

namespace Pokedex.PokeApiClient.Models
{
    public class FlavorTextEntry
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; set; }

        public NamedApiResource Language { get; set; }
    }
}
