using LuckyBid.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;

namespace LuckyBid.Infrastructure.Services;

public class PaystackService : IPaystackService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public PaystackService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        var secretKey = _configuration["Paystack:SecretKey"];
        if (!string.IsNullOrEmpty(secretKey))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
        }
    }

    public async Task<string> InitializeTransactionAsync(string email, decimal amount, string reference)
    {
        var request = new
        {
            email = email,
            amount = (int)(amount * 100), // Paystack uses kobo
            reference = reference
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.paystack.co/transaction/initialize", content);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseString);
            return document.RootElement.GetProperty("data").GetProperty("authorization_url").GetString() ?? string.Empty;
        }

        return string.Empty;
    }

    public async Task<bool> VerifyTransactionAsync(string reference)
    {
        var response = await _httpClient.GetAsync($"https://api.paystack.co/transaction/verify/{reference}");

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseString);
            var status = document.RootElement.GetProperty("data").GetProperty("status").GetString();
            return status == "success";
        }

        return false;
    }
}
