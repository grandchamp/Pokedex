using Microsoft.Extensions.Options;
using Pokedex.PokeApiClient.Configuration;
using Pokedex.PokeApiClient.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pokedex.PokeApiClient
{
    public class PokeApiClient : IPokeApiClient
    {
        private readonly HttpClient _httpClient;
        public PokeApiClient(HttpClient httpClient, IOptionsSnapshot<PokeApiClientConfiguration> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        public Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            return _httpClient.GetFromJsonAsync<Pokemon>($"pokemon/{name}");
        }

        public Task<T> RequestByNamedApiResource<T>(NamedApiResource resource)
        {
            return _httpClient.GetFromJsonAsync<T>(resource.Url);
        }
    }
}
