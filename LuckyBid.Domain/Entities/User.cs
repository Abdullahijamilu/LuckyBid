using System;
using System.Collections.Generic;
using LuckyBid.Domain.Enums;

namespace LuckyBid.Domain.Entities;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public MerchantProfile? MerchantProfile { get; set; }
    public ICollection<Slot> Slots { get; set; } = new List<Slot>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Winner> Wins { get; set; } = new List<Winner>();
}
