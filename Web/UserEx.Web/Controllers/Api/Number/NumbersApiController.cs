using System.Linq;

namespace UserEx.Web.Controllers.Api.Number
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using UserEx.Data;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("[controller]/api/number")]
    public class NumbersApiController : ControllerBase
    {
        private readonly ApplicationDbContext data;
        private readonly IConfiguration config;

        public NumbersApiController(
            ApplicationDbContext data,
            IConfiguration config)
        {
            this.config = config;
            this.data = data;
        }

        public async Task<IActionResult> UploadDids()
        {
            var didwwApiKey = this.config["Didww:ApiKey"];

            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.didww.com/v3/dids")
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
                        var numberData = new Data.Models.Number
                        {
                            ProviderId = 1,
                            DidNumber = number.Attributes.Number,
                            SetupPrice = 50,
                            MonthlyPrice = 50,
                            Description = number.Attributes.Description,
                            IsActive = true,
                            StartDate = number.Attributes.CreatedAt.Date,
                        };

                        numbersApiCollected.Add(numberData);
                    }

                    this.data.Numbers.AddRange(numbersApiCollected);

                    this.data.SaveChanges();
                }

                return Ok();
            }
        }
    }
}
