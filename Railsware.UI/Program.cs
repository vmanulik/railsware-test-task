using Microsoft.Extensions.DependencyInjection;
using Railsware.MailtrapClient;
using Railsware.MailtrapClient.Mail;
using Railsware.UI;
using System.Net.Http.Headers;

internal class Program
{
    private static ConfigManager ConfigManager { get; set; }

    private static IServiceProvider ServiceProvider { get; set; }

    private static bool Configure()
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
            return false;
        }

        // register default httpclient header values for reuse
        var services = new ServiceCollection();
        services.AddHttpClient("mailtrap", (serviceProvider, client) =>
        {
            // auth token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigManager.ApiConfig.BearerToken);

            client.BaseAddress = new Uri(ConfigManager.ApiConfig.SendEndpoint);
        });

        // register mailtrap client implementation
        ServiceProvider = services
        .AddSingleton<IMailClient, MailClient>()
        .BuildServiceProvider();

        return true;
    }

    static async Task Main()
    {
        bool isConfigured = Configure();
        if (!isConfigured)
        {
            Console.WriteLine("Application failed to configure itself");
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

        MailResult result = await ServiceProvider.GetService<IMailClient>().SendAsync(message);
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