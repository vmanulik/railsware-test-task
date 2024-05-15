namespace Railsware.UI
{
    public class Config
    {
        public required string SenderName { get; set; }
        public required string SenderEmail { get; set; }
        public required string RecipientName { get; set; }
        public required string RecipientEmail { get; set; }
        public required string Subject { get; set; }
        public required string Text { get; set; }
        public required string Html { get; set; }
        public required string[] AttachmentFiles { get; set; }
    }
}