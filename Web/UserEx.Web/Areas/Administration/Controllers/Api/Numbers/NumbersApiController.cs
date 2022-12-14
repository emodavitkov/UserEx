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
    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("[controller]/api/number")]
    public class NumbersApiController : AdministrationController
    {
        private readonly IConfiguration config;
        private readonly INumberApiService number;

        public NumbersApiController(
            IConfiguration config,
            INumberApiService number)
        {
            this.config = config;
            this.number = number;
        }

        public async Task<IActionResult> UploadDids()
        {
            var didwwApiKey = this.config["Didww:ApiKey"];
            var didwwId = 9;
            var result = 0;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.didww.com/v3/dids?include=order")
                    {
                        Headers =
                        {
                            { "Host", "api.didww.com" },
                            { "Accept", "application/vnd.api+json" },
                            { "Api-Key", didwwApiKey },
                        },
                    };

                    var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                        var numbersApiResponse =
                            await JsonSerializer.DeserializeAsync<NumbersApiResponseModel>(contentStream);

                        var numbersApiCollected = new List<Number>();

                        foreach (var number in numbersApiResponse.Data)
                        {
                            var currentNumber = number.Attributes.Number;
                            if (this.number.NumberExists(currentNumber))
                            {
                                continue;
                            }

                            var orderId = number.Relationships.Order.OrderData.OrderId;

                            var numberData = new Number
                            {
                                ProviderId = didwwId,
                                DidNumber = number.Attributes.Number,
                                SetupPrice = numbersApiResponse.Included.FirstOrDefault(o => o.IncludedOrderId == orderId).Attributes.OrderItems.FirstOrDefault().OrderItemsAttributes.SetupPrice,
                                MonthlyPrice = numbersApiResponse.Included.FirstOrDefault(o => o.IncludedOrderId == orderId).Attributes.OrderItems.FirstOrDefault().OrderItemsAttributes.MonthlyPrice,
                                OrderReference = numbersApiResponse.Included.FirstOrDefault(o => o.IncludedOrderId == orderId).Attributes.OrderReference,
                                Description = number.Attributes.Description,
                                Source = numbersApiResponse.Source,
                                IsActive = true,
                                StartDate = number.Attributes.CreatedAt.Date,
                            };

                            numbersApiCollected.Add(numberData);
                        }

                        result = numbersApiCollected.Count;

                        this.number.Add(numbersApiCollected);
                    }

                    if (result == 0)
                    {
                        return this.Ok("No new numbers added - your DIDww DID numbers are up-to-date! Go back and enjoy using UserEx!");
                    }
                    else
                    {
                        return this.Ok($"{@result} DIDww numbers added and awaiting for an approval! Go back and enjoy using UserEx!");
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
