using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cookbook.Services.EmailSender
{
    public class DevMessageSender : IMessageSender
    {
        private ILogger<DevMessageSender> _log;

        public DevMessageSender(ILogger<DevMessageSender> logger)
        {
            _log = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            _log.LogInformation(message);

            return Task.FromResult(0);
        }
    } 
}