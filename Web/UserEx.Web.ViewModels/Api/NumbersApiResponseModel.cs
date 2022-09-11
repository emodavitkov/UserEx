namespace UserEx.Web.ViewModels.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    public class NumbersApiResponseModel
    {
        [JsonPropertyName("data")]
        public ApiData[] Data { get; set; }


        public class ApiData
        {
            [JsonPropertyName("id")]
            public string Id { get; init; }

            [JsonPropertyName("attributes")]
            public ApiAttributes Attributes { get; set; }

            public class ApiAttributes
            {
                [JsonPropertyName("number")]
                public string Number { get; set; }

                [JsonPropertyName("description")]
                public string Description { get; set; }

                [JsonPropertyName("created_at")]
                public DateTime CreatedAt { get; set; }
            }
        }
    }
}
