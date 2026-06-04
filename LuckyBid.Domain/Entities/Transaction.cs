using System;
using LuckyBid.Domain.Enums;

namespace LuckyBid.Domain.Entities;

public class Transaction
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    
    public string ListingId { get; set; } = string.Empty;
    public Listing Listing { get; set; } = null!;
    
    public decimal Amount { get; set; }
    public string PaystackRef { get; set; } = string.Empty;
    
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
