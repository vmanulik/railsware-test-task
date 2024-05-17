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
                    Byte[] bytes = File.ReadAllBytes(file);
                    string content = Convert.ToBase64String(bytes);
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

        MailResult result = MailClient.Send(message, ConfigManager.ApiConfig.SendEndpoint, ConfigManager.ApiConfig.BearerToken);
        if (result.Success)
        {
            Console.WriteLine("All good so far!");
        }
        else
        {
            Console.WriteLine("Mail message failed to sent");
            return;
        }
    }
}