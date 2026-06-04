using System;

namespace LuckyBid.Domain.Entities;

public class Winner
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string ListingId { get; set; } = string.Empty;
    public Listing Listing { get; set; } = null!;
    
    public string BuyerId { get; set; } = string.Empty;
    public User Buyer { get; set; } = null!;
    
    public DateTime DrawnAt { get; set; } = DateTime.UtcNow;
    public bool NotificationSent { get; set; } = false;
}
