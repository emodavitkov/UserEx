namespace UserEx.Web.Areas.Administration.Controllers.Api.Balance
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.Extensions.Configuration;
    using UserEx.Data;
    using UserEx.Web.ViewModels.Api;

    // [Route("[controller]/api/balance")]

    [ApiController]
    [Route("api/[controller]")]
    public class BalancesApiController : AdministrationController
    {
        private readonly ApplicationDbContext data;
        private readonly IConfiguration config;

        public BalancesApiController(
            ApplicationDbContext data,
            IConfiguration config)
        {
            this.config = config;
            this.data = data;
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
