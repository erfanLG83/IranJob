using System;
using System.Net;
using System.Threading.Tasks;
using IranJob.Services.Contract;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace IranJob.Services.Implementation
{
    public class EmailSender : IEmailSender
    {
        readonly IConfiguration configuration;
        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [Obsolete]
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string fromEmail = configuration["Email:UserName"];
            string password = configuration["Email:Password"];
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };
            MimeMessage mimeMessage = new MimeMessage
            {
                Subject = subject,
                Body = bodyBuilder.ToMessageBody()
            };
            mimeMessage.From.Add(new MailboxAddress(fromEmail));
            mimeMessage.To.Add(new MailboxAddress(email));
            using (SmtpClient client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(new NetworkCredential(fromEmail, password));
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
            await Task.CompletedTask;
        }
    }
}
