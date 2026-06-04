using LuckyBid.Domain.Entities;
using System.Threading.Tasks;

namespace LuckyBid.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
