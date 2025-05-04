using Ecom.Application.DTOs;
using Ecom.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Ecom.Application.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //SMTP
        public async Task SendEmail(EmailDTO emailDTO)
        {

            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("My Ecom", configuration["EmailSettings:From"]));

            message.Subject = emailDTO.Subject;

            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    // Connect to the SMTP server
                    await smtp.ConnectAsync(
                        configuration["EmailSettings:Smtp"], 
                        int.Parse(configuration["EmailSettings:Port"]),true);

                    // Authenticate with the server
                    await smtp.AuthenticateAsync(
                        configuration["EmailSettings:UserName"],
                        configuration["EmailSettings:Password"]);

                    // Send the email
                    await smtp.SendAsync(message);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    // Disconnect and Dispose
                    await smtp.DisconnectAsync(true);
                    smtp.Dispose();
                }
            }
        }
    }
}
