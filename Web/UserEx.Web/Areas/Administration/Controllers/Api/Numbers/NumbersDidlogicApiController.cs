namespace UserEx.Web.Areas.Administration.Controllers.Api.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("[controller]/api/number")]
    public class NumbersDidlogicApiController : AdministrationController
    {
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
            var result = 0;

            try
            {
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

                        var numbersApiCollected = new List<Number>();

                        foreach (var number in numbersApiResponse.Purchases)
                        {
                            // move to service
                            var currentNumber = number.Number;
                            if (this.number.NumberExists(currentNumber))
                            {
                                continue;
                            }

                            var numberData = new Number
                            {
                                ProviderId = didlogicId,
                                DidNumber = number.Number,
                                Description = number.Description,
                                MonthlyPrice = number.MonthlyPrice,
                                SetupPrice = number.SetupPrice,
                                OrderReference = null,
                                Source = numbersApiResponse.Source,
                                IsActive = true,
                                StartDate = DateTime.Now,
                            };
                            numbersApiCollected.Add(numberData);
                        }

                        result = numbersApiCollected.Count;
                        this.number.Add(numbersApiCollected);
                    }

                    if (result == 0)
                    {
                        return this.Ok("No new numbers added - your DidLogic DID numbers are up-to-date! Go back and enjoy using UserEx!");
                    }
                    else
                    {
                        return this.Ok($"{@result} DidLogic numbers added and awaiting for an approval! Go back and enjoy using UserEx!");
                    }
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
