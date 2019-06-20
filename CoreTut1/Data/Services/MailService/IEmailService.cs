using System.Threading.Tasks;

namespace Data.Services.MailService
{
    public interface IEmailService
    {
        bool SendEmail(string email, string body, string template);
        Task<bool> SendMailAsync(string email, string body, string template);
    }
}
