using Railsware.MailtrapClient.Mail;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Railsware.MailtrapClient
{
    public class MailClient
    {
        public static MailResult Send(MailMessage message, string url, string bearerToken)
        {
           using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                // auth token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                // add custom serialization for enums
                var deserializeOptions = new JsonSerializerOptions();
                deserializeOptions.Converters.Add(new DispositionJsonConverter());

                string json = JsonSerializer.Serialize(message, typeof(MailMessage), deserializeOptions);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResult = client.PostAsync(url, data).Result;

                return httpResult.Content.ReadFromJsonAsync<MailResult>().Result;
            }
        }
    }
}