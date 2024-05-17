using Railsware.MailtrapClient.Mail;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Railsware.MailtrapClient
{
    public class MailClient : IMailClient
    {
        private readonly IHttpClientFactory _factory;

        public MailClient(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<MailResult> SendAsync(MailMessage message)
        {
           using(HttpClient client = _factory.CreateClient("mailtrap"))
            {
                // add custom serialization for enums
                var deserializeOptions = new JsonSerializerOptions();
                deserializeOptions.Converters.Add(new DispositionJsonConverter());

                string json = JsonSerializer.Serialize(message, typeof(MailMessage), deserializeOptions);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResult = await client.PostAsync("send", data);

                return await httpResult.Content.ReadFromJsonAsync<MailResult>();
            }
        }
    }
}