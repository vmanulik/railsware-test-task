using System.Text.Json.Serialization;
using System.Text.Json;
using Railsware.MailtrapClient.Mail;

namespace Railsware.MailtrapClient
{
    public class DispositionJsonConverter : JsonConverter<MailAttachmentDisposition>
    {
        public override MailAttachmentDisposition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string formattedValue = FirstCharToUpper(reader.GetString());

            if(Enum.TryParse(formattedValue, out MailAttachmentDisposition parsedValue))
            {
                return parsedValue;
            }
            else
            {
                throw new JsonException("MailAttachmentDisposition value could not be converted");
            }
        }

        public override void Write(Utf8JsonWriter writer, MailAttachmentDisposition enumValue, JsonSerializerOptions options)
        {
            writer.WriteStringValue(enumValue.ToString().ToLower());
        }

        private string FirstCharToUpper(string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input[0].ToString().ToUpper() + input.Substring(1)
            };
    }
}