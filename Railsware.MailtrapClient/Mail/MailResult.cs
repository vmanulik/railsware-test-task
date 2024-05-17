namespace Railsware.MailtrapClient.Mail
{
    public class MailResult
    {
        public bool Success { get; set; }

        public string[] Message_Ids { get; set; }

        public string[] Errors { get; set; }
    }
}
