using Pokedex.Domain.Entities;
using Pokedex.Infrastructure;
using Xunit;

namespace Pokedex.UnitTests.Helpers
{
    public class PokemonTransformationActionsExtensionsTests
    {
        [Fact]
        public void GetAllActions_NoneIsProvided_ReturnEmptyEnumerable()
        {
            var actions = PokemonTransformationActions.None;

            var result = actions.GetAllActions();

            Assert.Empty(result);
        }

        [Fact]
        public void GetAllActions_OneActionProvided_ReturnAction()
        {
            var action = PokemonTransformationActions.TranslateDescription;

            var result = action.GetAllActions();

            Assert.Single(result, action);
        }
    }
}
