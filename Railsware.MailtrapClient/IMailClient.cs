using Railsware.MailtrapClient.Mail;

namespace Railsware.MailtrapClient
{
    public interface IMailClient
    {
        Task<MailResult> SendAsync(MailMessage message);
    }
}