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
    public class YodaDescriptionPokemonTransformerTests
    {
        [Theory]
        [InlineData("cave", false)]
        [InlineData("cave", true)]
        [InlineData("rare", true)]
        public async Task Transform_TranslationSuccess(string habitat, bool isLegendary)
        {
            const string EXPECTED_DESCRIPTION = "translated";

            var mockTranslationClient = new Mock<IFunTranslationsClient>();
            mockTranslationClient.Setup(x => x.TranslateText(It.IsAny<string>(), TranslationType.Yoda))
                                 .ReturnsAsync(EXPECTED_DESCRIPTION);

            var sut = new YodaDescriptionPokemonTransformer(mockTranslationClient.Object,
                                                            NullLogger<YodaDescriptionPokemonTransformer>.Instance);

            var pokemon = new Pokemon("poke", "description", habitat, isLegendary);
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
            mockTranslationClient.Setup(x => x.TranslateText(It.IsAny<string>(), TranslationType.Yoda))
                                 .ThrowsAsync(new Exception());

            var sut = new YodaDescriptionPokemonTransformer(mockTranslationClient.Object,
                                                            NullLogger<YodaDescriptionPokemonTransformer>.Instance);

            var pokemon = new Pokemon("poke", "description", "rare", false);
            var result = await sut.Transform(pokemon);

            Assert.Equal(result.Description, result.Description);
            Assert.Equal(pokemon.Name, result.Name);
            Assert.Equal(pokemon.Habitat, result.Habitat);
            Assert.Equal(pokemon.IsLegendary, result.IsLegendary);
        }

        [Fact]
        public async Task Transform_PokemonNotValidForTransformation_ReturnSamePokemon()
        {
            var mockTranslationClient = new Mock<IFunTranslationsClient>();

            var sut = new YodaDescriptionPokemonTransformer(mockTranslationClient.Object,
                                                            NullLogger<YodaDescriptionPokemonTransformer>.Instance);

            var pokemon = new Pokemon("poke", "description", "rare", false);
            var result = await sut.Transform(pokemon);

            Assert.Equal(pokemon.Description, result.Description);
            Assert.Equal(pokemon.Name, result.Name);
            Assert.Equal(pokemon.Habitat, result.Habitat);
            Assert.Equal(pokemon.IsLegendary, result.IsLegendary);
        }
    }
}
