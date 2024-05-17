using Railsware.UI;

namespace Railsware.Tests
{
    [TestClass]
    public class ConfigManagerTests
    {
        [TestMethod]
        public void LoadValidFile()
        {
            string configFilePath = "./TestFiles/messageconfig.test.valid.json";
            string apiFilePath = "./TestFiles/apiconfig.json";

            var configManager = new ConfigManager();
            configManager.Load(configFilePath, apiFilePath);

            Assert.IsNotNull(configManager.MessageConfig);
            Assert.IsNotNull(configManager.MessageConfig.SenderName);
            Assert.IsNotNull(configManager.MessageConfig.SenderEmail);
            Assert.IsNotNull(configManager.MessageConfig.RecipientName);
            Assert.IsNotNull(configManager.MessageConfig.RecipientEmail);
            Assert.IsNotNull(configManager.MessageConfig.Subject);
            Assert.IsNotNull(configManager.MessageConfig.Text);
            Assert.IsNotNull(configManager.MessageConfig.Html);
            Assert.IsNotNull(configManager.MessageConfig.AttachmentFiles);
            Assert.IsTrue(configManager.MessageConfig.AttachmentFiles.Length > 1);
        }

        [TestMethod]
        public void LoadNoFile()
        {
            // not a real file
            string configFilePath = "./TestFiles/messageconfig.no.file.json";
            string apiFilePath = "./TestFiles/apiconfig.json";

            var configManager = new ConfigManager();
            Action act = () => configManager.Load(configFilePath, apiFilePath);

            Assert.ThrowsException<FileNotFoundException>(act);
        }

        [TestMethod]
        public void LoadInvalidNameFile()
        {
            string configFilePath = "./TestFiles/messageconfig.test.invalid.name.json";
            string apiFilePath = "./TestFiles/apiconfig.json";

            var configManager = new ConfigManager();
            Action act = () => configManager.Load(configFilePath, apiFilePath);

            Assert.ThrowsException<FileLoadException>(act);
        }

        [TestMethod]
        public void LoadInvalidValueFile()
        {
            string configFilePath = "./TestFiles/messageconfig.test.invalid.array.json";
            string apiFilePath = "./TestFiles/apiconfig.json";

            var configManager = new ConfigManager();
            Action act = () => configManager.Load(configFilePath, apiFilePath);

            Assert.ThrowsException<FileLoadException>(act);
        }
    }
}