namespace UserEx.Web.Areas.Administration.Controllers.Api.Balance
{
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("api/[controller]")]
    public class BalancesSquareTalkApiController : AdministrationController
    {
        private readonly IConfiguration config;

        public BalancesSquareTalkApiController(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<IActionResult> GetBalance()
        {
            var squaretalkApiKey = this.config["SquareTalk:ApiKey"];

            var balance = new BalancesSquareTalkApiResponseModel { };

            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage =
                    new HttpRequestMessage(HttpMethod.Get,
                        $"https://portal.squaretalk.com/api/balance?api_token={@squaretalkApiKey}")
                    {
                        Headers =
                        {
                            { "Accept", "application/vnd.api+json" },
                        },
                    };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                var result = string.Empty;

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    var balanceSquareTalkApiResponse =
                        await JsonSerializer.DeserializeAsync<BalancesSquareTalkApiResponseModel>(contentStream);

                    result = balanceSquareTalkApiResponse.Data.FirstOrDefault().BalanceSquareTalkAmount;
                }

                balance = new BalancesSquareTalkApiResponseModel()
                {
                   BalanceAmount = result,
                };
                return this.Ok(balance);
            }
        }
    }
}
