using Railsware.UI;

namespace Railsware.Tests
{
    [TestClass]
    public class ConfigManagerTests
    {
        [TestMethod]
        public void LoadValidFile()
        {
            string configFilePath = "./appsettings.test.valid.json";

            Config config = new ConfigManager().Load(configFilePath);

            Assert.IsNotNull(config);
            Assert.IsNotNull(config.SenderName);
            Assert.IsNotNull(config.SenderEmail);
            Assert.IsNotNull(config.RecipientName);
            Assert.IsNotNull(config.RecipientEmail);
            Assert.IsNotNull(config.Subject);
            Assert.IsNotNull(config.Text);
            Assert.IsNotNull(config.Html);
            Assert.IsNotNull(config.AttachmentFiles);
            Assert.IsTrue(config.AttachmentFiles.Length > 1);
        }

        [TestMethod]
        public void LoadNoFile()
        {
            // not a real file
            string configFilePath = "./appsettings.no.file.json"; 

            Action act = () => new ConfigManager().Load(configFilePath);

            Assert.ThrowsException<FileNotFoundException>(act);
        }

        [TestMethod]
        public void LoadInvalidNameFile()
        {
            string configFilePath = "./appsettings.test.invalid.name.json";

            Action act = () => new ConfigManager().Load(configFilePath);

            Assert.ThrowsException<FileLoadException>(act);
        }

        [TestMethod]
        public void LoadInvalidValueFile()
        {
            string configFilePath = "./appsettings.test.invalid.array.json";

            Action act = () => new ConfigManager().Load(configFilePath);

            Assert.ThrowsException<FileLoadException>(act);
        }
    }
}