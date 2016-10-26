namespace Cookbook.Services.EmailSender
{
    public class AuthMessageSenderOptions
    {
        public string SendGridKey { get; set; }
        public string Address { get; set; } = "noreply@cookbook.com";
        public string DisplayName { get; set; } = "Cookbook Account";
    }
}
