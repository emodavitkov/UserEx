namespace UserEx.Web.ViewModels.Api
{
    using System;
    using System.Text.Json.Serialization;

    using UserEx.Data.Models;

    public class NumbersDidlogicApiResponseModel
    {
        [JsonPropertyName("purchases")]
        public ApiPurchases[] Purchases { get; set; }

        public SourceEnum Source { get; set; } = (SourceEnum)1;

        public class ApiPurchases
        {
            [JsonPropertyName("number")]
            public string Number { get; set; }

            [JsonPropertyName("area")]
            public string Description { get; set; }

            [JsonPropertyName("activation")]
            public decimal SetupPrice { get; set; }

            [JsonPropertyName("monthly_fee")]
            public decimal MonthlyPrice { get; set; }
        }
    }
}
