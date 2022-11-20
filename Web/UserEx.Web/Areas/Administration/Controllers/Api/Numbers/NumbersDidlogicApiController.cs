namespace UserEx.Web.Areas.Administration.Controllers.Api.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using UserEx.Services.Data.Numbers;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("[controller]/api/number")]
    public class NumbersDidlogicApiController : AdministrationController
    {
       // private readonly ApplicationDbContext data;
        private readonly IConfiguration config;
        private readonly INumberDidlogicApiService number;

        public NumbersDidlogicApiController(
            IConfiguration config,
            INumberDidlogicApiService number)
        {
            this.config = config;
            this.number = number;
        }

        public async Task<IActionResult> UploadDids()
        {
            var didlogicApiKey = this.config["Didlogic:ApiKey"];
            var didlogicId = 3;

            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://didlogic.com/api/v1/purchases.json?apiid={@didlogicApiKey}")
                {
                    Headers =
                    {
                        { "Host", "didlogic.com" },
                    },
                };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    var numbersApiResponse =
                        await JsonSerializer.DeserializeAsync<NumbersDidlogicApiResponseModel>(contentStream);

                    var numbersApiCollected = new List<Data.Models.Number>();

                    foreach (var number in numbersApiResponse.Purchases)
                    {
                        // move to service
                        var currentNumber = number.Number;
                        if (this.number.NumberExists(currentNumber))
                        {
                            continue;
                        }

                        var numberData = new Data.Models.Number
                        {
                            ProviderId = didlogicId,
                            DidNumber = number.Number,
                            Description = number.Description,
                            MonthlyPrice = number.MonthlyPrice,
                            SetupPrice = number.SetupPrice,
                            //SetupPrice = numbersApiResponse.Purchases.Select(x => x.SetupPrice).FirstOrDefault(),
                            // MonthlyPrice = numbersApiResponse.Purchases.Select(x => x.MonthlyPrice).FirstOrDefault(),
                            OrderReference = null,
                            // Description = numbersApiResponse.Purchases.Select(x => x.Description).FirstOrDefault(),
                            Source = numbersApiResponse.Source,
                            IsActive = true,
                            StartDate = DateTime.Now,
                        };
                        numbersApiCollected.Add(numberData);
                    }

                    this.number.Add(numbersApiCollected);
                }

                return this.Ok();
            }
        }
    }
}
