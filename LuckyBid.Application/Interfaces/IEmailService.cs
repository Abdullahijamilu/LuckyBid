using System.Threading.Tasks;

namespace LuckyBid.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
