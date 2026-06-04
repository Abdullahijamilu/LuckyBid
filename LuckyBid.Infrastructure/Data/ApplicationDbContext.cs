using Microsoft.EntityFrameworkCore;
using LuckyBid.Domain.Entities;

namespace LuckyBid.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<MerchantProfile> MerchantProfiles { get; set; } = null!;
    public DbSet<Listing> Listings { get; set; } = null!;
    public DbSet<Slot> Slots { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Winner> Winners { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<PlatformSetting> PlatformSettings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MerchantProfile>()
            .HasKey(mp => mp.UserId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.MerchantProfile)
            .WithOne(mp => mp.User)
            .HasForeignKey<MerchantProfile>(mp => mp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Listing>()
            .HasOne(l => l.Merchant)
            .WithMany()
            .HasForeignKey(l => l.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Slot>()
            .HasOne(s => s.Listing)
            .WithMany(l => l.Slots)
            .HasForeignKey(s => s.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Slot>()
            .HasOne(s => s.Buyer)
            .WithMany(b => b.Slots)
            .HasForeignKey(s => s.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Listing)
            .WithMany()
            .HasForeignKey(t => t.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Winner>()
            .HasOne(w => w.Listing)
            .WithOne(l => l.Winner)
            .HasForeignKey<Winner>(w => w.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Winner>()
            .HasOne(w => w.Buyer)
            .WithMany(b => b.Wins)
            .HasForeignKey(w => w.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlatformSetting>()
            .HasKey(ps => ps.Key);
    }
}
