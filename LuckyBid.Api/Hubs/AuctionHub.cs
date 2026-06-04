using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LuckyBid.Api.Hubs;

public class AuctionHub : Hub
{
    public async Task JoinListingGroup(string listingId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Listing_{listingId}");
    }

    public async Task LeaveListingGroup(string listingId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Listing_{listingId}");
    }
}
