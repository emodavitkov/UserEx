﻿namespace UserEx.Web.Areas.Administration.Controllers.Api.Numbers
{
    // UserEx.Web.Areas.Administration.Controllers.Api.Numbers
    // UserEx.Web.Controllers.Api.Number
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using UserEx.Data;
    using UserEx.Services.Data.Numbers;
    using UserEx.Web.ViewModels.Api;

    // public class NumbersApiController : ControllerBase
    [ApiController]
    [Route("[controller]/api/number")]
    public class NumbersApiController : AdministrationController
    {
       // private readonly ApplicationDbContext data;
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
            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.didww.com/v3/dids?include=order")
                {
                    Headers =
                    {
                        { "Host", "api.didww.com" },

                        // { "Content-Type", "application/vnd.api+json" },
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

                    var numbersApiCollected = new List<Data.Models.Number>();

                    foreach (var number in numbersApiResponse.Data)
                    {
                        // move to service
                        var currentNumber = number.Attributes.Number;
                        if (this.number.NumberExists(currentNumber))
                        {
                            continue;
                        }

                        // if (this.data.Numbers.FirstOrDefault(n => n.DidNumber == number.Attributes.Number) != null)
                        // {
                        //    continue;
                        // }
                        var orderId = number.Relationships.Order.OrderData.OrderId;

                        var numberData = new Data.Models.Number
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


                        // foreach (var orderDetails in numbersApiResponse.Included)
                        // {
                        //    if (orderDetails.IncludedOrderId == orderId)
                        //    {
                        //        numberData.MonthlyPrice = orderDetails.Attributes.OrderItems.FirstOrDefault().OrderItemsAttributes.MonthlyPrice,
                        //        numberData.SetupPrice = orderDetails.Attributes.OrderItems.OrderItemsAttributes.OrderSetupPrice,
                        //            OrderReference = orderDetails.Attributes.OrderReference,

                        // };
                        //    }
                        // }
                        numbersApiCollected.Add(numberData);
                    }

                    // move to service
                    this.number.Add(numbersApiCollected);

                    // this.data.Numbers.AddRange(numbersApiCollected);
                    // this.data.SaveChanges();
                }

                return this.Ok();
            }
        }
    }
}
