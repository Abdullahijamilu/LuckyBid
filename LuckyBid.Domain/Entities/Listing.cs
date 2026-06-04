using System;
using System.Collections.Generic;
using LuckyBid.Domain.Enums;

namespace LuckyBid.Domain.Entities;

public class Listing
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string MerchantId { get; set; } = string.Empty;
    public User Merchant { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new List<string>();
    
    public decimal ItemValue { get; set; }
    public int TotalSlots { get; set; }
    public decimal SlotPrice { get; set; }
    public int SlotsFilled { get; set; } = 0;
    
    public ListingStatus Status { get; set; } = ListingStatus.Draft;
    public DateTime StartDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Slot> Slots { get; set; } = new List<Slot>();
    public Winner? Winner { get; set; }
}
