using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Pokedex.Domain.Adapters;
using Pokedex.Domain.Entities;
using Pokedex.Domain.Transformers;
using Pokedex.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.UnitTests.Services
{
    public class PokemonServiceTests
    {
        [Fact]
        public async Task GetPokemonAsync_TransformationActionsIsNone_DoNotTransform()
        {
            var fakeDataAdapter = new Mock<IPokemonDataAdapter>();
            fakeDataAdapter.Setup(x => x.GetPokemonByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(Result.Success(new Pokemon("poke", "pokemon", "none", false)));

            var fakeTransformer = new Mock<IPokemonTransformer>();
            fakeTransformer.SetupGet(x => x.TransformationType)
                           .Returns(PokemonTransformationActions.TranslateDescription);
            fakeTransformer.Setup(x => x.IsValidForTransformation(It.IsAny<Pokemon>()))
                           .Returns(true);

            var transformations = new Dictionary<PokemonTransformationActions, IEnumerable<IPokemonTransformer>>
            {
                [PokemonTransformationActions.TranslateDescription] = new List<IPokemonTransformer>
                {
                    fakeTransformer.Object
                }
            };

            var sut = new PokemonService(fakeDataAdapter.Object, transformations, NullLogger<PokemonService>.Instance);

            var result = await sut.GetPokemonAsync("poke", PokemonTransformationActions.None);

            Assert.True(result.IsSuccess);
            fakeTransformer.Verify(x => x.Transform(It.IsAny<Pokemon>()), Times.Never());
        }

        [Fact]
        public async Task GetPokemonAsync_TransformationActionsIsTranslateDescription_TransformResult()
        {
            const string EXPECTED_TRANSFORMED_DESCRIPTION = "transformed";
            var pokemon = new Pokemon("poke", "pokemon", "none", false);

            var fakeDataAdapter = new Mock<IPokemonDataAdapter>();
            fakeDataAdapter.Setup(x => x.GetPokemonByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(Result.Success(pokemon));

            var fakeTransformer = new Mock<IPokemonTransformer>();
            fakeTransformer.SetupGet(x => x.TransformationType)
                           .Returns(PokemonTransformationActions.TranslateDescription);
            fakeTransformer.Setup(x => x.IsValidForTransformation(It.IsAny<Pokemon>()))
                           .Returns(true);
            fakeTransformer.Setup(x => x.Transform(It.IsAny<Pokemon>()))
                           .ReturnsAsync(new Pokemon(pokemon.Name, EXPECTED_TRANSFORMED_DESCRIPTION, pokemon.Habitat, pokemon.IsLegendary));

            var transformations = new Dictionary<PokemonTransformationActions, IEnumerable<IPokemonTransformer>>
            {
                [PokemonTransformationActions.TranslateDescription] = new List<IPokemonTransformer>
                {
                    fakeTransformer.Object
                }
            };

            var sut = new PokemonService(fakeDataAdapter.Object, transformations, NullLogger<PokemonService>.Instance);

            var result = await sut.GetPokemonAsync("poke", PokemonTransformationActions.TranslateDescription);

            Assert.True(result.IsSuccess);
            Assert.Equal(EXPECTED_TRANSFORMED_DESCRIPTION, result.Value.Description);
        }
    }
}
