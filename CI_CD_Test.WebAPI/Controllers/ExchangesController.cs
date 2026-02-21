using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Xml.Linq;

namespace CI_CD_Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExchangesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            var client = _httpClientFactory.CreateClient();
            var xmlString = await client.GetStringAsync("https://www.tcmb.gov.tr/kurlar/today.xml");

            var xDoc = XDocument.Parse(xmlString);

            var date = xDoc.Root?.Attribute("Tarih")?.Value;

            var rates = xDoc.Descendants("Currency")
                .Select(c => new
                {
                    Code = c.Attribute("Kod")?.Value,
                    ForexBuying = (string)c.Element("ForexBuying"),
                    ForexSelling = (string)c.Element("ForexSelling"),
                    BanknoteBuying = (string)c.Element("BanknoteBuying"),
                    BanknoteSelling = (string)c.Element("BanknoteSelling")
                })
                .ToList();

            return Ok(new
            {
                Date = date,
                Rates = rates
            });
        }
    }
}
