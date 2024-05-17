using Railsware.MailtrapClient;
using Railsware.MailtrapClient.Mail;
using Railsware.UI;

internal class Program
{
    public static ConfigManager ConfigManager { get; private set; }

    private static void Main()
    {
        try
        {
            // load settings
            string applicationConfigFileName = Environment.GetCommandLineArgs()[1];
            string apiConfigFileName = Environment.GetCommandLineArgs()[2];
            ConfigManager = new ConfigManager();
            ConfigManager.Load(applicationConfigFileName, apiConfigFileName);
        }
        catch
        {
            Console.WriteLine("Error reading json config file");
            return;
        }

        // build message
        var message = new MailMessage()
        {
            From = new MailAddress(ConfigManager.MessageConfig.SenderEmail, ConfigManager.MessageConfig.SenderEmail),
            To = new List<MailAddress>()
                {
                     new MailAddress(ConfigManager.MessageConfig.RecipientName, ConfigManager.MessageConfig.RecipientEmail)
                },
            Subject = ConfigManager.MessageConfig.Subject,
            Text = ConfigManager.MessageConfig.Text,
            Html = ConfigManager.MessageConfig.Html
        };

        // try load attachments
        if (ConfigManager.MessageConfig.AttachmentFiles.Any())
        {
            message.Attachments = new List<MailAttachment>();

            foreach (string file in ConfigManager.MessageConfig.AttachmentFiles)
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine("Error loading attachment files");
                    return;
                }
                else
                {
                    string content = File.ReadAllText(file);
                    message.Attachments.Add(new MailAttachment(content, file));
                }
            }
        }

        bool isValid = message.Validate();
        if (!isValid)
        {
            Console.WriteLine("Mail message is not valid");
            return;
        }
    }
}