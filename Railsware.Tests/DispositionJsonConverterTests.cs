using Railsware.MailtrapClient.Mail;
using Railsware.MailtrapClient;
using System.Text.Json;

namespace Railsware.Tests
{
    [TestClass]
    public class DispositionJsonConverterTests
    {
        [TestMethod]
        public void SerializeEnum()
        {
            var attachment = new MailAttachment("file.txt", "./TestFiles/file.txt");
            attachment.Disposition = MailAttachmentDisposition.Inline;

            var deserializeOptions = new JsonSerializerOptions();
            deserializeOptions.Converters.Add(new DispositionJsonConverter());
            string json = JsonSerializer.Serialize(attachment, typeof(MailAttachment), deserializeOptions);

            Assert.IsNotNull(json);
            Assert.IsTrue(json.Contains("inline"));
        }

        [TestMethod]
        public void DeserializeEnum()
        {
            string json = "{\"Content\":\"file.txt\",\"Type\":null,\"FileName\":\"./TestFiles/file.txt\",\"Disposition\":\"inline\",\"Content_Id\":null}";

            var deserializeOptions = new JsonSerializerOptions();
            deserializeOptions.Converters.Add(new DispositionJsonConverter());
            MailAttachment attachment = (MailAttachment) JsonSerializer.Deserialize(json, typeof(MailAttachment), deserializeOptions);

            Assert.IsNotNull(attachment);
            Assert.IsTrue(attachment.Disposition == MailAttachmentDisposition.Inline);
        }
    }
}