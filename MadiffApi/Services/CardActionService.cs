namespace Services
{
    public class CardActionService : ICardActionService
    {
        public IEnumerable<CardAction> GetAllowedActions(CardDetails card)
        {
            var allowedActions = new List<CardAction>();
            
            allowedActions.AddRange([ 
                CardAction.ACTION3, 
                CardAction.ACTION4,
                CardAction.ACTION8,
                CardAction.ACTION9,
                CardAction.ACTION10
            ]);

            if (card.CardStatus != CardStatus.ORDERED && card.CardStatus != CardStatus.CLOSED)
            {
                allowedActions.Add(CardAction.ACTION1);
            }

            if (card.CardStatus != CardStatus.ORDERED && 
                card.CardStatus != CardStatus.RESTRICTED && 
                card.CardStatus != CardStatus.BLOCKED && 
                card.CardStatus != CardStatus.CLOSED)
            {
                allowedActions.Add(CardAction.ACTION2);
            }

            if (card.CardType != CardType.PREPAID)
            {
                allowedActions.Add(CardAction.ACTION5);
            }

            if (ShouldAllowPinDependentActions(card))
            {
                allowedActions.Add(CardAction.ACTION6);
                allowedActions.Add(CardAction.ACTION7);
            }

            if (card.CardStatus != CardStatus.BLOCKED && 
                card.CardStatus != CardStatus.EXPIRED && 
                card.CardStatus != CardStatus.CLOSED)
            {
                allowedActions.Add(CardAction.ACTION11);
            }

            if (card.CardStatus != CardStatus.CLOSED)
            {
                allowedActions.Add(CardAction.ACTION12);
                allowedActions.Add(CardAction.ACTION13);
            }

            return allowedActions;
        }

        private bool ShouldAllowPinDependentActions(CardDetails card)
        {
            if (card.CardStatus == CardStatus.RESTRICTED || 
                card.CardStatus == CardStatus.EXPIRED || 
                card.CardStatus == CardStatus.CLOSED)
            {
                return false;
            }

            if (card.CardStatus == CardStatus.BLOCKED)
            {
                return card.IsPinSet;
            }

            if (card.CardStatus == CardStatus.ORDERED || card.CardStatus == CardStatus.INACTIVE)
            {
                return !card.IsPinSet;
            }

            return true;
        }
    }
} 