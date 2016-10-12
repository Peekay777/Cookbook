using System.Threading.Tasks;

namespace Cookbook.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}