using Microsoft.Extensions.Options;
using SendGrid;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cookbook.Services
{
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; }

        public AuthMessageSender(IOptions<AuthMessageSenderOptions> options)
        {
            Options = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            var myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new MailAddress("noreply@cookbook.com", "Cookbook Account");
            myMessage.Subject = subject;
            myMessage.Text = message;
            myMessage.Html = message;

            // Create a Web transport for sending email.
            var transportWeb = new Web(Options.SendGridKey);
            return transportWeb.DeliverAsync(myMessage);
        }
    }
}
