using System.Collections.Generic;

namespace UserEx.Web.Controllers.Api.Number
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("[controller]/api/number")]
    public class NumbersApiController : ControllerBase
    {
        private readonly ApplicationDbContext data;

        public NumbersApiController(ApplicationDbContext data)
        {
            this.data = data;
        }

        //[Route("api/upload")]
        public async Task<IActionResult> UploadDids()
        {
            var apiKey = "3MZ92NURQ7UU8ZBX8VRX8CSRZOSJ3MT";

            using (var httpClient = new HttpClient())
            {

                // https://api.didww.com/v3/dids?page%5Bnumber%5D=1&page%5Bsize%5D=50",

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.didww.com/v3/dids")
                {
                    Headers =
                    {
                        { "Host", "api.didww.com" },

                        // { "Content-Type", "application/vnd.api+json" },
                        { "Accept", "application/vnd.api+json" },
                        { "Api-Key", apiKey },
                    },
                };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    //  using var contentStream = httpResponseMessage.Content.ReadAsStream();

                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    var numbersApiResponse =
                        await JsonSerializer.DeserializeAsync<NumbersApiResponseModel>(contentStream);

                    var numbers = new List<Data.Models.Number>();

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

                        numbers.Add(numberData);
                    }

                    this.data.Numbers.AddRange(numbers);

                    this.data.SaveChanges();
                }

                return Ok();
            }
        }
    }
}
