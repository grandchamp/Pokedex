using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Pokedex.Domain.Entities;
using Pokedex.FunTranslationsClient;
using Pokedex.FunTranslationsClient.Models;
using Pokedex.Infrastructure.Transformers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.UnitTests.Transformers
{
    public class ShakespeareDescriptionPokemonTransformerTests
    {
        [Fact]
        public async Task Transform_TranslationSuccess()
        {
            const string EXPECTED_DESCRIPTION = "translated";

            var mockTranslationClient = new Mock<IFunTranslationsClient>();
            mockTranslationClient.Setup(x => x.TranslateText(It.IsAny<string>(), TranslationType.Shakespeare))
                                 .ReturnsAsync(EXPECTED_DESCRIPTION);

            var sut = new ShakespeareDescriptionPokemonTransformer(mockTranslationClient.Object,
                                                                   NullLogger<ShakespeareDescriptionPokemonTransformer>.Instance);

            var pokemon = new Pokemon("poke", "description", "rare", false);
            var result = await sut.Transform(pokemon);

            Assert.Equal(EXPECTED_DESCRIPTION, result.Description);
            Assert.Equal(pokemon.Name, result.Name);
            Assert.Equal(pokemon.Habitat, result.Habitat);
            Assert.Equal(pokemon.IsLegendary, result.IsLegendary);
        }

        [Fact]
        public async Task Transform_TranslationFailed_ReturnSamePokemon()
        {
            var mockTranslationClient = new Mock<IFunTranslationsClient>();
            mockTranslationClient.Setup(x => x.TranslateText(It.IsAny<string>(), TranslationType.Shakespeare))
                                 .ThrowsAsync(new Exception());

            var sut = new ShakespeareDescriptionPokemonTransformer(mockTranslationClient.Object,
                                                                   NullLogger<ShakespeareDescriptionPokemonTransformer>.Instance);

            var pokemon = new Pokemon("poke", "description", "rare", false);
            var result = await sut.Transform(pokemon);

            Assert.Equal(result.Description, result.Description);
            Assert.Equal(pokemon.Name, result.Name);
            Assert.Equal(pokemon.Habitat, result.Habitat);
            Assert.Equal(pokemon.IsLegendary, result.IsLegendary);
        }

        [Theory]
        [InlineData("cave", false)]
        [InlineData("cave", true)]
        [InlineData("rare", true)]
        public async Task Transform_PokemonNotValidForTransformation_ReturnSamePokemon(string habitat, bool isLegendary)
        {
            var mockTranslationClient = new Mock<IFunTranslationsClient>();

            var sut = new ShakespeareDescriptionPokemonTransformer(mockTranslationClient.Object,
                                                                   NullLogger<ShakespeareDescriptionPokemonTransformer>.Instance);

            var pokemon = new Pokemon("poke", "description", habitat, isLegendary);
            var result = await sut.Transform(pokemon);

            Assert.Equal(pokemon.Description, result.Description);
            Assert.Equal(pokemon.Name, result.Name);
            Assert.Equal(pokemon.Habitat, result.Habitat);
            Assert.Equal(pokemon.IsLegendary, result.IsLegendary);
        }
    }
}
