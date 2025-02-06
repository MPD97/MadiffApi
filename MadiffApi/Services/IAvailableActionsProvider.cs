namespace MadiffApi.Services
{
    public interface IAvailableActionsProvider
    {
        Task<IEnumerable<CardAction>> GetAvailableActions(string userId, string cardNumber);
    }
}
