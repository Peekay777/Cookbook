using System.Threading.Tasks;

namespace Cookbook.Services.EmailSender
{
    public interface IMessageSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}