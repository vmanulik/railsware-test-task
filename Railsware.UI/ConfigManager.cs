using Microsoft.Extensions.Configuration;

namespace Railsware.UI
{
    public class ConfigManager
    {
        public Config Load(string configFilePath)
        {
            Config config;

            if(!File.Exists(configFilePath))
            {
                throw new FileNotFoundException();
            }

            using (Stream jsonStream = new FileStream(configFilePath, FileMode.Open))
            {
                IConfigurationRoot configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonStream(jsonStream)
                .Build();

                config = configBuilder.Get<Config>();
            }

            // check if any field is empty
            if (config == null 
                // for string options
                || config.GetType().GetProperties()
                    .Where(pi => pi.PropertyType == typeof(string))
                    .Select(pi => (string) pi.GetValue(config))
                    .Any(val => string.IsNullOrEmpty(val))
                // for array option
                || config.GetType().GetProperties()
                .Where(pi => pi.PropertyType == typeof(string[]))
                .Select(pi => (string[]) pi.GetValue(config))
                .Any(val => val == null || val.Length < 1))
            {
                throw new FileLoadException();
            }

            return config;
        }
    }
}
