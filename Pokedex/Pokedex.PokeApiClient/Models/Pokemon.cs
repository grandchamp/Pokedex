namespace Pokedex.PokeApiClient.Models
{
    public class Pokemon
    {
        public string Name { get; set; }
        public NamedApiResource Species { get; set; }
    }
}
