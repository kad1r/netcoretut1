using System;
using System.Threading.Tasks;

namespace Data.Services.MailService
{
    public class EmailService : IEmailService
    {
        public bool SendEmail(string email, string body, string template)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendMailAsync(string email, string body, string template)
        {
            throw new NotImplementedException();
        }
    }
}
