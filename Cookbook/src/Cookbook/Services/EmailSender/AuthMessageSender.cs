using Microsoft.Extensions.Options;
using SendGrid;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cookbook.Services.EmailSender
{
    public class AuthMessageSender : IMessageSender
    {
        public AuthMessageSenderOptions _options { get; }

        public AuthMessageSender(IOptions<AuthMessageSenderOptions> options)
        {
            _options = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            var myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            //myMessage.From = new MailAddress("noreply@cookbook.com", "Cookbook Account");
            myMessage.From = new MailAddress(_options.Address, _options.DisplayName);
            myMessage.Subject = subject;
            myMessage.Text = message;
            myMessage.Html = message;

            // Create a Web transport for sending email.
            var transportWeb = new Web(_options.SendGridKey);
            return transportWeb.DeliverAsync(myMessage);
        }
    }
}
