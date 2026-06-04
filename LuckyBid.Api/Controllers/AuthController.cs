using LuckyBid.Application.Interfaces;
using LuckyBid.Domain.Entities;
using LuckyBid.Domain.Enums;
using LuckyBid.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LuckyBid.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = request.Password, // Demo purposes: use proper hashing
            Role = request.Role,
            Status = request.Role == UserRole.Merchant ? UserStatus.Pending : UserStatus.Active
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registration successful. " + (user.Status == UserStatus.Pending ? "Pending admin approval." : ""));
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || user.PasswordHash != request.Password)
            return Unauthorized("Invalid credentials");

        if (user.Status != UserStatus.Active)
            return Forbid("Account is not active");

        var token = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        return Ok(new { Token = token, RefreshToken = refreshToken });
    }
}
