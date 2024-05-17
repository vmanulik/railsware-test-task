using Railsware.MailtrapClient.Mail;

namespace Railsware.Tests
{
    [TestClass]
    public class MailValidationTests
    {
        [TestMethod]
        public void ValidateValidMessage()
        {
            var message = new MailMessage()
            {
                From = new MailAddress("Mr. Test", "mr.test@mail.com"),
                To = new List<MailAddress>()
                {
                     new MailAddress("Mr. Test", "mr.test@mail.com")
                },
                Subject = "Test Subject",
                Text = "Text",
                Html = "Html",
                Attachments = new List<MailAttachment>()
                {
                    new MailAttachment("attachment.txt", "./TestFiles/attachment.txt")
                }
            };

            bool isValid = message.Validate();

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateNoRequiredFieldsMessage()
        {
            var message = new MailMessage()
            {
                To = new List<MailAddress>()
                {
                     new MailAddress("Mr. Test", "mr.test@mail.com")
                },
                Subject = "Test Subject",
                Text = "Text",
                Html = "Html"
            };

            bool isValid = message.Validate();

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateWrongLenthFieldsMessage()
        {
            var message = new MailMessage()
            {
                From = new MailAddress("Mr. Test", "mr.test@mail.com"),
                To = new List<MailAddress>()
                {
                     new MailAddress("Mr. Test", "mr.test@mail.com")
                },
                Subject = "Test Subject",
                Text = "",
                Html = "Html"
            };

            bool isValid = message.Validate();

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateAbsentBodyFieldsMessage()
        {
            var message = new MailMessage()
            {
                From = new MailAddress("Mr. Test", "mr.test@mail.com"),
                To = new List<MailAddress>()
                {
                     new MailAddress("Mr. Test", "mr.test@mail.com")
                },
                Subject = "Test Subject"
            };

            bool isValid = message.Validate();

            Assert.IsFalse(isValid);
        }
    }
}