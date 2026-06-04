using LuckyBid.Api.Hubs;
using LuckyBid.Application.Interfaces;
using LuckyBid.Domain.Entities;
using LuckyBid.Domain.Enums;
using LuckyBid.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LuckyBid.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IPaystackService _paystackService;
    private readonly IHubContext<AuctionHub> _hubContext;

    public ListingsController(ApplicationDbContext context, IPaystackService paystackService, IHubContext<AuctionHub> hubContext)
    {
        _context = context;
        _paystackService = paystackService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetListings()
    {
        var listings = await _context.Listings
            .Where(l => l.Status == ListingStatus.Active)
            .ToListAsync();
        return Ok(listings);
    }

    public class CreateListingRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal ItemValue { get; set; }
        public int TotalSlots { get; set; }
    }

    [Authorize(Roles = "Merchant")]
    [HttpPost]
    public async Task<IActionResult> CreateListing(CreateListingRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var listing = new Listing
        {
            MerchantId = userId,
            Title = request.Title,
            Description = request.Description,
            ItemValue = request.ItemValue,
            TotalSlots = request.TotalSlots,
            SlotPrice = request.ItemValue / request.TotalSlots,
            Status = ListingStatus.Active,
            StartDate = DateTime.UtcNow
        };

        _context.Listings.Add(listing);
        await _context.SaveChangesAsync();

        return Ok(listing);
    }

    [Authorize(Roles = "Buyer")]
    [HttpPost("{id}/buy-slot")]
    public async Task<IActionResult> BuySlot(string id)
    {
        var listing = await _context.Listings.FindAsync(id);
        if (listing == null || listing.Status != ListingStatus.Active)
            return NotFound("Listing not found or not active");

        if (listing.SlotsFilled >= listing.TotalSlots)
            return BadRequest("Auction is already filled");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        
        var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "test@example.com";
        var reference = $"tx_{Guid.NewGuid():N}";

        var authUrl = await _paystackService.InitializeTransactionAsync(email, listing.SlotPrice, reference);
        
        var transaction = new Transaction
        {
            UserId = userId,
            ListingId = id,
            Amount = listing.SlotPrice,
            PaystackRef = reference
        };
        
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(new { AuthorizationUrl = authUrl, Reference = reference });
    }
}
