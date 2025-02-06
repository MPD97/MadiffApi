namespace MadiffApi.Repositories;
public interface ICardService
{
    Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
}