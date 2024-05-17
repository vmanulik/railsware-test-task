using System.ComponentModel.DataAnnotations;

namespace Railsware.MailtrapClient.Mail
{
    public class MailAttachment
    {
        [Required]
        [MinLength(1)]
        public string Content { get; set; }

        [MinLength(1)]
        public string Type { get; set; }

        [Required]
        public string FileName { get; set; }

        public MailAttachmentDisposition Disposition { get; set; }

        public string ContentId { get; set; }

        public MailAttachment(string content, string fileName)
        {
            Content = content;
            FileName = fileName;

            // default value for disposition
            Disposition = MailAttachmentDisposition.Attachment;
        }
    }
}