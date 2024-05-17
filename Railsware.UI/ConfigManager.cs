using Microsoft.Extensions.Configuration;

namespace Railsware.UI
{

    public class ConfigManager
    {
        public MessageConfig MessageConfig { get; private set; }
        public ApiConfig ApiConfig { get; private set; }

        public void Load(string applicationConfigFilePath, string apiConfigFilePath)
        {
            if (!File.Exists(applicationConfigFilePath) || !File.Exists(apiConfigFilePath))
            {
                throw new FileNotFoundException();
            }

            // load message values
            using (Stream jsonStream = new FileStream(applicationConfigFilePath, FileMode.Open))
            {
                IConfigurationRoot configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonStream(jsonStream)
                .Build();

                MessageConfig = configBuilder.Get<MessageConfig>();
            }

            // check if any field is empty
            if (!IsValid(MessageConfig))
            {
                throw new FileLoadException();
            }

            // load api values
            using (Stream jsonStream = new FileStream(apiConfigFilePath, FileMode.Open))
            {
                IConfigurationRoot configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonStream(jsonStream)
                .Build();

                ApiConfig = configBuilder.Get<ApiConfig>();
            }

            // check if any field is empty
            if (!IsValid(ApiConfig))
            {
                throw new FileLoadException();
            }
        }

        private bool IsValid(object config)
        {
            return config != null
                    // for string options
                    && !config.GetType().GetProperties()
                        .Where(pi => pi.PropertyType == typeof(string))
                        .Select(pi => (string) pi.GetValue(config))
                        .Any(val => string.IsNullOrEmpty(val))
                    // for array option
                    && !config.GetType().GetProperties()
                    .Where(pi => pi.PropertyType == typeof(string[]))
                    .Select(pi => (string[]) pi.GetValue(config))
                    .Any(val => val == null || val.Length < 1);
        }
    }
}
