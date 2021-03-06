using Microsoft.Extensions.Options;
using Pokedex.FunTranslationsClient.Configuration;
using Pokedex.FunTranslationsClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.FunTranslationsClient
{
    public class FunTranslationsClient : IFunTranslationsClient
    {
        private readonly HttpClient _httpClient;
        private readonly FunTranslationsClientConfiguration _options;
        public FunTranslationsClient(HttpClient httpClient, IOptions<FunTranslationsClientConfiguration> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> TranslateText(string text, TranslationType translationType)
        {
            var endpoint = GetTranslationEndpoint(translationType);

            var parameters = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["text"] = text
            });

            var response = await _httpClient.PostAsync($"{_options.BaseUrl}/{endpoint}", parameters);

            response.EnsureSuccessStatusCode();

            var rawJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TranslationResponse>(rawJson,
                                                                         new JsonSerializerOptions
                                                                         {
                                                                             PropertyNameCaseInsensitive = true
                                                                         });

            return result.Contents.Translated;
        }

        private string GetTranslationEndpoint(TranslationType translationType)
            => _options.TranslationTypeEndpoints[translationType.ToString()];
    }
}
