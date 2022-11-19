namespace UserEx.Web.ViewModels.Api
{
    using System.Text.Json.Serialization;

    public class BalancesDidwwApiResponseModel
    {
        public string BalanceAmount { get; set; }

        [JsonPropertyName("data")]
        public ApiData Data { get; set; }

        public class ApiData
        {
            [JsonPropertyName("attributes")]
            public ApiAttributes Attributes { get; set; }

            public class ApiAttributes
            {
                [JsonPropertyName("balance")]
                public string BalanceDidwwAmount { get; set; }
            }
        }
    }
}
