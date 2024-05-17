using System.ComponentModel.DataAnnotations;

namespace Railsware.MailtrapClient.Mail
{
    public class MailAddress
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public MailAddress (string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
