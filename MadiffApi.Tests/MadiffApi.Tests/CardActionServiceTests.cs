using FluentAssertions;
using Madiff.Api.Models;
using Services;

namespace MadiffApi.Tests
{
    public class CardActionServiceTests
    {
        private readonly CardActionService _sut;

        public CardActionServiceTests()
        {
            _sut = new CardActionService();
        }

        [Theory]
        [InlineData(CardStatus.ORDERED, CardType.CREDIT, false)]  // Card11
        [InlineData(CardStatus.INACTIVE, CardType.CREDIT, true)]  // Card12
        [InlineData(CardStatus.ACTIVE, CardType.CREDIT, false)]   // Card13
        [InlineData(CardStatus.RESTRICTED, CardType.CREDIT, true)] // Card14
        public void GetAllowedActions_ForCreditCards_ReturnsExpectedActions(CardStatus status, CardType type, bool isPinSet)
        {
            // Arrange
            var card = new CardDetails("TestCard", type, status, isPinSet);

            // Act
            var result = _sut.GetAllowedActions(card);

            // Assert
            result.Should().Contain(CardAction.ACTION3);
            result.Should().Contain(CardAction.ACTION4);
            result.Should().Contain(CardAction.ACTION5);
            result.Should().Contain(CardAction.ACTION8);
            result.Should().Contain(CardAction.ACTION9);
            result.Should().Contain(CardAction.ACTION10);
        }

        [Theory]
        [InlineData(CardStatus.ORDERED, false)]     // Card11
        [InlineData(CardStatus.CLOSED, true)]       // Card17
        public void GetAllowedActions_ForOrderedOrClosedStatus_ShouldNotContainAction1(CardStatus status, bool isPinSet)
        {
            // Arrange
            var card = new CardDetails("TestCard", CardType.CREDIT, status, isPinSet);

            // Act
            var result = _sut.GetAllowedActions(card);

            // Assert
            result.Should().NotContain(CardAction.ACTION1);
        }

        [Theory]
        [InlineData(CardType.PREPAID, CardStatus.ACTIVE, false)]  // Testing ACTION5 restriction
        public void GetAllowedActions_ForPrepaidCards_ShouldNotContainAction5(CardType type, CardStatus status, bool isPinSet)
        {
            // Arrange
            var card = new CardDetails("TestCard", type, status, isPinSet);

            // Act
            var result = _sut.GetAllowedActions(card);

            // Assert
            result.Should().NotContain(CardAction.ACTION5);
        }

        [Theory]
        [InlineData(CardStatus.ORDERED, false, true)]     // Card11
        [InlineData(CardStatus.INACTIVE, false, true)]    // Card19
        [InlineData(CardStatus.BLOCKED, true, true)]      // Card14
        [InlineData(CardStatus.RESTRICTED, true, false)]  // Card14
        public void GetAllowedActions_PinDependentActions_ReturnsExpectedActions(CardStatus status, bool isPinSet, bool shouldContainPinActions)
        {
            // Arrange
            var card = new CardDetails("TestCard", CardType.CREDIT, status, isPinSet);

            // Act
            var result = _sut.GetAllowedActions(card);

            // Assert
            if (shouldContainPinActions)
            {
                result.Should().Contain(CardAction.ACTION6);
                result.Should().Contain(CardAction.ACTION7);
            }
            else
            {
                result.Should().NotContain(CardAction.ACTION6);
                result.Should().NotContain(CardAction.ACTION7);
            }
        }

        [Theory]
        [InlineData(CardStatus.BLOCKED)]
        [InlineData(CardStatus.EXPIRED)]
        [InlineData(CardStatus.CLOSED)]
        public void GetAllowedActions_ForRestrictedStatuses_ShouldNotContainAction11(CardStatus status)
        {
            // Arrange
            var card = new CardDetails("TestCard", CardType.CREDIT, status, false);

            // Act
            var result = _sut.GetAllowedActions(card);

            // Assert
            result.Should().NotContain(CardAction.ACTION11);
        }

        [Fact]
        public void GetAllowedActions_ForClosedStatus_ShouldNotContainActions12And13()
        {
            // Arrange
            var card = new CardDetails("TestCard", CardType.CREDIT, CardStatus.CLOSED, false);

            // Act
            var result = _sut.GetAllowedActions(card);

            // Assert
            result.Should().NotContain(CardAction.ACTION12);
            result.Should().NotContain(CardAction.ACTION13);
        }
    }
}
