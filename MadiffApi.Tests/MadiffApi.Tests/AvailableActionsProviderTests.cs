using FluentAssertions;
using Madiff.Api.Models;
using MadiffApi.Exceptions;
using MadiffApi.Repositories;
using MadiffApi.Services;
using Moq;

namespace MadiffApi.Tests
{
    public class AvailableActionsProviderTests
    {
        private readonly Mock<ICardService> _cardServiceMock;
        private readonly Mock<ICardActionService> _cardActionServiceMock;
        private readonly AvailableActionsProvider _sut;

        public AvailableActionsProviderTests()
        {
            _cardServiceMock = new Mock<ICardService>();
            _cardActionServiceMock = new Mock<ICardActionService>();
            _sut = new AvailableActionsProvider(_cardServiceMock.Object, _cardActionServiceMock.Object);
        }

        [Fact]
        public async Task GetAvailableActions_WhenCardExists_ReturnsAllowedActions()
        {
            // Arrange
            var userId = "User1";
            var cardNumber = "1234567890";
            var card = new CardDetails(cardNumber, CardType.CREDIT, CardStatus.ACTIVE, false);
            var expectedActions = new List<CardAction>
            {
                CardAction.ACTION1,
                CardAction.ACTION2,
                CardAction.ACTION3
            };

            _cardServiceMock
                .Setup(x => x.GetCardDetails(userId, cardNumber))
                .ReturnsAsync(card);

            _cardActionServiceMock
                .Setup(x => x.GetAllowedActions(card))
                .Returns(expectedActions);

            // Act
            var result = await _sut.GetAvailableActions(userId, cardNumber);

            // Assert
            result.Should().BeEquivalentTo(expectedActions);
            _cardServiceMock.Verify(x => x.GetCardDetails(userId, cardNumber), Times.Once);
            _cardActionServiceMock.Verify(x => x.GetAllowedActions(card), Times.Once);
        }

        [Fact]
        public async Task GetAvailableActions_WhenCardDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = "User1";
            var cardNumber = "1234567890";

            _cardServiceMock
                .Setup(x => x.GetCardDetails(userId, cardNumber))
                .ReturnsAsync((CardDetails)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _sut.GetAvailableActions(userId, cardNumber));

            _cardServiceMock.Verify(x => x.GetCardDetails(userId, cardNumber), Times.Once);
            _cardActionServiceMock.Verify(x => x.GetAllowedActions(It.IsAny<CardDetails>()), Times.Never);
        }
    }
}
