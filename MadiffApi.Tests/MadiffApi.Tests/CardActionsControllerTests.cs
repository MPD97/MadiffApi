using FluentAssertions;
using MadiffApi.Controllers;
using MadiffApi.Requests;
using MadiffApi.Responses;
using MadiffApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MadiffApi.Tests
{
    public class CardActionsControllerTests
    {
        private readonly Mock<IAvailableActionsProvider> _availableActionsProviderMock;
        private readonly CardActionsController _controller;

        public CardActionsControllerTests()
        {
            _availableActionsProviderMock = new Mock<IAvailableActionsProvider>();
            _controller = new CardActionsController(_availableActionsProviderMock.Object);
        }

        [Fact]
        public async Task ProcessAvailableCardActions_WhenValidRequest_ReturnsOkWithActions()
        {
            // Arrange
            var request = new CardActionRequest
            {
                UserId = "User1",
                CardNumber = "1234567890"
            };
            var expectedActions = new List<CardAction>
            {
               CardAction.ACTION1,
               CardAction.ACTION2,
            };

            _availableActionsProviderMock
                .Setup(x => x.GetAvailableActions(request.UserId, request.CardNumber))
                .ReturnsAsync(expectedActions);

            // Act
            var result = await _controller.ProcessAvailableCardActions(request);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<CardActionResponse>().Subject;
            response.CardActions.Should().BeEquivalentTo(expectedActions);
        }

        [Fact]
        public async Task ProcessAvailableCardActions_WhenProviderReturnsEmpty_ReturnsOkWithEmptyList()
        {
            // Arrange
            var request = new CardActionRequest
            {
                UserId = "User1",
                CardNumber = "1234567890"
            };
            var expectedActions = new List<CardAction>();
            _availableActionsProviderMock
                .Setup(x => x.GetAvailableActions(request.UserId, request.CardNumber))
                .ReturnsAsync(expectedActions);

            // Act
            var result = await _controller.ProcessAvailableCardActions(request);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<CardActionResponse>().Subject;
            response.CardActions.Should().BeEmpty();
        }
    }
}
