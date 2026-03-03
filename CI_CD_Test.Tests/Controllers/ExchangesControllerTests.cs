using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CI_CD_Test.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CI_CD_Test.Tests.Controllers;

public class ExchangesControllerTests
{
    [Fact]
    public async Task GetToday_ReturnsOkResult()
    {
        var xml = """
                  <Tarih_Date Tarih="01.01.2024">
                    <Currency Kod="USD">
                      <ForexBuying>30.00</ForexBuying>
                      <ForexSelling>31.00</ForexSelling>
                      <BanknoteBuying>29.50</BanknoteBuying>
                      <BanknoteSelling>31.50</BanknoteSelling>
                    </Currency>
                  </Tarih_Date>
                  """;

        var controller = CreateControllerWithResponse(xml);

        var result = await controller.GetToday();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    private static ExchangesController CreateControllerWithResponse(string xmlResponse)
    {
        var services = new ServiceCollection();

        services.AddHttpClient(string.Empty)
            .ConfigurePrimaryHttpMessageHandler(() => new StubHttpMessageHandler(xmlResponse));

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IHttpClientFactory>();

        return new ExchangesController(factory);
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _content;

        public StubHttpMessageHandler(string content)
        {
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_content)
            };

            return Task.FromResult(response);
        }
    }
}

