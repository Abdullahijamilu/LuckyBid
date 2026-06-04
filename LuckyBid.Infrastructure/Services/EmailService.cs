using LuckyBid.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LuckyBid.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Simple SMTP implementation
        var host = _configuration["Email:Host"];
        var port = int.Parse(_configuration["Email:Port"] ?? "587");
        var user = _configuration["Email:Username"];
        var pass = _configuration["Email:Password"];
        var from = _configuration["Email:From"] ?? "no-reply@luckybid.com";

        if (string.IsNullOrEmpty(host)) 
        {
            Console.WriteLine($"[EmailService Stub] Sent to {to}: {subject}");
            return;
        }

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, pass),
            EnableSsl = true
        };

        var mailMessage = new MailMessage(from, to, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(mailMessage);
    }
}
