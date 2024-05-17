using Moq;
using Moq.Protected;
using Railsware.MailtrapClient;
using Railsware.MailtrapClient.Mail;
using System.Net.Http.Json;

namespace Railsware.Tests
{
    [TestClass]
    public class MailClientTests
    {
        private Mock<HttpMessageHandler> _handlerMock;
        private IMailClient _client;

        [TestInitialize]
        public void Setup()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            var result = new HttpResponseMessage();
            result.Content = JsonContent.Create(new MailResult());

            this._handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .Returns(Task.FromResult(result))
            .Verifiable()            ;

            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://send.api.mailtrap.io/api/")
            };

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient("mailtrap")).Returns(httpClient);

            _client = new MailClient(mockHttpClientFactory.Object);
        }

        [TestMethod]
        public async Task ValidateValidMessage()
        {
            await _client.SendAsync(new MailMessage());

            _handlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}