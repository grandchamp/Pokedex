using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Domain.Entities;
using Pokedex.Domain.Services;
using System.Threading.Tasks;

namespace Pokedex.Api.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("{pokemonName}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Pokemon), StatusCodes.Status200OK)]
        public Task<IActionResult> Get(string pokemonName)
            => GetPokemonBase(pokemonName, PokemonTransformationActions.None);

        [HttpGet("translated/{pokemonName}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Pokemon), StatusCodes.Status200OK)]
        public Task<IActionResult> GetTranslated(string pokemonName)
            => GetPokemonBase(pokemonName, PokemonTransformationActions.TranslateDescription);

        private async Task<IActionResult> GetPokemonBase(string pokemonName, PokemonTransformationActions actions)
        {
            if (string.IsNullOrEmpty(pokemonName))
                return BadRequest();

            var pokemon = await _pokemonService.GetPokemonAsync(pokemonName, actions);

            if (pokemon.IsFailure)
                return NotFound();

            return Ok(pokemon.Value);
        }
    }
}
