using MadiffApi.Exceptions;

namespace MadiffApi.Services
{
    public class AvailableActionsProvider : IAvailableActionsProvider
    {
        private readonly ICardService _cardService;
        private readonly ICardActionService _cardActionService;

        public AvailableActionsProvider(ICardService cardService, ICardActionService cardActionService)
        {
            _cardService = cardService;
            _cardActionService = cardActionService;
        }
        public async Task<IEnumerable<CardAction>> GetAvailableActions(string userId, string cardNumber)
        {
            var card = await _cardService.GetCardDetails(userId, cardNumber);
            if (card == null)
            {
                throw new NotFoundException($"Card not found for user {userId}");
            }

            return _cardActionService.GetAllowedActions(card);
        }
    }
}
