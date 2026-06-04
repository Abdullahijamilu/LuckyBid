using LuckyBid.Api.Hubs;
using LuckyBid.Application.Interfaces;
using LuckyBid.Domain.Entities;
using LuckyBid.Domain.Enums;
using LuckyBid.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyBid.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IPaystackService _paystackService;
    private readonly IHubContext<AuctionHub> _hubContext;

    public PaymentsController(ApplicationDbContext context, IPaystackService paystackService, IHubContext<AuctionHub> hubContext)
    {
        _context = context;
        _paystackService = paystackService;
        _hubContext = hubContext;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> PaystackWebhook([FromBody] dynamic payload)
    {
        // In production: verify X-Paystack-Signature header here
        
        string eventType = payload.GetProperty("event").GetString();
        if (eventType == "charge.success")
        {
            string reference = payload.GetProperty("data").GetProperty("reference").GetString();
            
            var transaction = await _context.Transactions
                .Include(t => t.Listing)
                .FirstOrDefaultAsync(t => t.PaystackRef == reference);
                
            if (transaction != null && transaction.Status == TransactionStatus.Pending)
            {
                var isVerified = await _paystackService.VerifyTransactionAsync(reference);
                if (isVerified)
                {
                    transaction.Status = TransactionStatus.Success;
                    
                    var slot = new Slot
                    {
                        ListingId = transaction.ListingId,
                        BuyerId = transaction.UserId,
                        PaymentReference = reference
                    };
                    
                    _context.Slots.Add(slot);
                    
                    var listing = transaction.Listing;
                    listing.SlotsFilled++;
                    
                    await _hubContext.Clients.Group($"Listing_{listing.Id}")
                        .SendAsync("SlotPurchased", new { listing.Id, listing.SlotsFilled });
                        
                    if (listing.SlotsFilled >= listing.TotalSlots)
                    {
                        listing.Status = ListingStatus.Filled;
                        await _hubContext.Clients.Group($"Listing_{listing.Id}")
                            .SendAsync("AuctionFilled", listing.Id);
                            
                        // Trigger Draw Logic (simple random for demo)
                        var slots = await _context.Slots.Where(s => s.ListingId == listing.Id).ToListAsync();
                        var winnerSlot = slots[new Random().Next(slots.Count)];
                        
                        var winner = new Winner
                        {
                            ListingId = listing.Id,
                            BuyerId = winnerSlot.BuyerId
                        };
                        _context.Winners.Add(winner);
                        
                        await _hubContext.Clients.Group($"Listing_{listing.Id}")
                            .SendAsync("WinnerDrawn", winner.BuyerId);
                    }
                    
                    await _context.SaveChangesAsync();
                }
            }
        }
        
        return Ok();
    }
}
