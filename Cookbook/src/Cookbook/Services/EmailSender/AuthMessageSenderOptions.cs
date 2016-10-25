namespace Cookbook.Services.EmailSender
{
    public class AuthMessageSenderOptions
    {
        public string SendGridKey { get; set; }
        public string Address { get; set; }
        public string DisplayName { get; set; }
    }
}
