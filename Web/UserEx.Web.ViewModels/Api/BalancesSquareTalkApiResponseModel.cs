namespace UserEx.Web.ViewModels.Api
{
    using System.Text.Json.Serialization;

    public class BalancesSquareTalkApiResponseModel
    {
        public string BalanceAmount { get; set; }

        [JsonPropertyName("data")]
        public ApiData[] Data { get; set; }

        public class ApiData
        {
                [JsonPropertyName("balance")]
                public string BalanceSquareTalkAmount { get; set; }
        }
    }
}
