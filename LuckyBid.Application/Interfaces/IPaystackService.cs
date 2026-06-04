using System.Threading.Tasks;

namespace LuckyBid.Application.Interfaces;

public interface IPaystackService
{
    Task<string> InitializeTransactionAsync(string email, decimal amount, string reference);
    Task<bool> VerifyTransactionAsync(string reference);
}
