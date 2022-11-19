namespace UserEx.Web.Areas.Administration.Controllers.Api.Balance
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("api/[controller]")]
    public class BalancesDidwwApiController : AdministrationController
    {
        private readonly IConfiguration config;

        public BalancesDidwwApiController(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<IActionResult> GetBalance()
        {
            var didwwApiKey = this.config["Didww:ApiKey"];
            var balance = new BalancesDidwwApiResponseModel { };

            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage =
                    new HttpRequestMessage(HttpMethod.Get,
                        $"https://api.didww.com/v3/balance")
                    {
                        Headers =
                        {
                            { "Host", "api.didww.com" },
                            { "Accept", "application/vnd.api+json" },
                            { "Api-Key", didwwApiKey },
                        },
                    };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                var result = string.Empty;

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    var balanceDidwwApiResponse =
                        await JsonSerializer.DeserializeAsync<BalancesDidwwApiResponseModel>(contentStream);

                    result = balanceDidwwApiResponse.Data.Attributes.BalanceDidwwAmount.ToString();
                }

                balance = new BalancesDidwwApiResponseModel()
                {
                    BalanceAmount = result,
                };
                return this.Ok(balance);
            }
        }
    }
}
