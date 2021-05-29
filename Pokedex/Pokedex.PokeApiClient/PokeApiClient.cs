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
        private readonly PokeApiClientConfiguration _configuration;
        public PokeApiClient(HttpClient httpClient, IOptions<PokeApiClientConfiguration> options)
        {
            _httpClient = httpClient;
            _configuration = options.Value;
        }

        public Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            return _httpClient.GetFromJsonAsync<Pokemon>($"{_configuration.BaseUrl}/pokemon/{name}");
        }

        public Task<T> RequestByNamedApiResource<T>(NamedApiResource resource)
        {
            return _httpClient.GetFromJsonAsync<T>(resource.Url);
        }
    }
}
