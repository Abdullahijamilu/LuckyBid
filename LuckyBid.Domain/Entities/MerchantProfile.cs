using System;

namespace LuckyBid.Domain.Entities;

public class MerchantProfile
{
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessDescription { get; set; } = string.Empty;
    public string DocumentUrl { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
}
