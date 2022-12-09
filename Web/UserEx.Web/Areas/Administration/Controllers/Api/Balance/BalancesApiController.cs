namespace UserEx.Web.Areas.Administration.Controllers.Api.Balance
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("api/[controller]")]
    public class BalancesApiController : AdministrationController
    {
        private readonly IConfiguration config;

        public BalancesApiController(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<IActionResult> GetBalance()
        {
            var didlogicApiKey = this.config["Didlogic:ApiKey"];

            var balance = new BalancesApiResponseModel { };

            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage =
                    new HttpRequestMessage(HttpMethod.Get,
                        $"https://didlogic.com/api/v1/balance.json?apiid={@didlogicApiKey}")
                    {
                        Headers =
                        {
                            { "Host", "didlogic.com" },
                        },
                    };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                var result = string.Empty;

                using (HttpContent content = httpResponseMessage.Content)
                {
                    result = content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                result = result.Remove(0, 11).TrimEnd('}');

                balance = new BalancesApiResponseModel
                {
                    BalanceAmount = result,
                };
            }

            return this.Ok(balance);
        }
    }
}
