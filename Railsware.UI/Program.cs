using Railsware.MailtrapClient.Mail;
using Railsware.UI;

internal class Program
{
    public static Config Config { get; private set; }

    private static void Main()
    {
        try
        {
            // load settings
            string configFileName = Environment.GetCommandLineArgs()[1];
            Config = new ConfigManager().Load(configFileName);
        }
        catch
        {
            Console.WriteLine("Error reading json config file");
            return;
        }

        // build message
        var message = new MailMessage()
        {
            From = new MailAddress(Config.SenderEmail, Config.SenderEmail),
            To = new List<MailAddress>()
                {
                     new MailAddress(Config.RecipientName, Config.RecipientEmail)
                },
            Subject = Config.Subject,
            Text = Config.Text,
            Html = Config.Html
        };

        // try load attachments
        if (Config.AttachmentFiles.Any())
        {
            message.Attachments = new List<MailAttachment>();

            foreach (string file in Config.AttachmentFiles)
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

        // validate
        bool isValid = MailValidator.TryValidate(message);
        if (!isValid)
        {
            Console.WriteLine("Mail message is not valid");
            return;
        }

        Console.WriteLine("All good so far!");
    }
}