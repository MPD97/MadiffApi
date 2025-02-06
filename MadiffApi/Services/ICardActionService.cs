public interface ICardActionService
{
    IEnumerable<CardAction> GetAllowedActions(CardDetails card);
} 