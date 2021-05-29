using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokedex.PokeApiClient.Models
{
    public class Species
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("flavor_text_entries")]
        public IEnumerable<FlavorTextEntry> FlavorTextEntries { get; set; }

        public NamedApiResource Habitat { get; set; }
    }
}
