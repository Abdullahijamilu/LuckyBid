using LuckyBid.Domain.Entities;
using LuckyBid.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyBid.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureCreatedAsync();

        if (!context.Users.Any(u => u.Role == UserRole.Admin))
        {
            var admin = new User
            {
                FullName = "Super Admin",
                Email = "admin@luckybid.com",
                // In a real app, use a password hasher like BCrypt or ASP.NET Core Identity's PasswordHasher
                PasswordHash = "hashed_password_here", 
                Role = UserRole.Admin,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(admin);
        }

        if (!context.PlatformSettings.Any())
        {
            context.PlatformSettings.AddRange(
                new PlatformSetting { Key = "CommissionPercentage", Value = "5" },
                new PlatformSetting { Key = "MaxSlotsPerBuyer", Value = "5" },
                new PlatformSetting { Key = "MinSlotPrice", Value = "1.00" }
            );
        }

        await context.SaveChangesAsync();
    }
}
