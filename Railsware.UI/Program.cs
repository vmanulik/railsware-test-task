using Microsoft.Extensions.Configuration;
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
        catch(Exception ex)
        {
            Console.WriteLine("Error reading json config file");
            return;
        }

        Console.WriteLine($"Hello, {Config.SenderName}!");
    }
}