using System;

namespace LuckyBid.Domain.Entities;

public class Slot
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string ListingId { get; set; } = string.Empty;
    public Listing Listing { get; set; } = null!;
    
    public string BuyerId { get; set; } = string.Empty;
    public User Buyer { get; set; } = null!;
    
    public string PaymentReference { get; set; } = string.Empty;
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
}
