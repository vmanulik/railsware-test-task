using System.ComponentModel.DataAnnotations;

namespace Railsware.MailtrapClient.Mail
{
    public class MailMessage
    {
        [Required]
        public MailAddress From { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public IEnumerable<MailAddress> To { get; set; }

        [MaxLength(1000)]
        public IEnumerable<MailAddress> Cc { get; set; }

        [MaxLength(1000)]
        public IEnumerable<MailAddress> Bcc { get; set; }

        public List<MailAttachment> Attachments { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> Custom_Variables { get; set; }

        [Required]
        public string Subject { get; set; }

        [MinLength(1)]
        public string Text { get; set; }

        [MinLength(1)]
        public string Html { get; set; }

        [MaxLength(255)]
        public string Category { get; set; }
    }
}
