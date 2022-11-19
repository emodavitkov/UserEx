namespace UserEx.Web.ViewModels.Api
{
    using System;
    using System.Text.Json.Serialization;

    using UserEx.Data.Models;

    public class NumbersApiResponseModel
    {
        [JsonPropertyName("data")]
        public ApiData[] Data { get; set; }

        [JsonPropertyName("included")]
        public ApiIncluded[] Included { get; set; }

        public SourceEnum Source { get; set; } = (SourceEnum)1;

        public class ApiData
        {
            [JsonPropertyName("id")]
            public string Id { get; init; }

            [JsonPropertyName("attributes")]
            public ApiAttributes Attributes { get; set; }

            [JsonPropertyName("relationships")]
            public ApiRelationships Relationships { get; set; }

            public class ApiAttributes
            {
                [JsonPropertyName("number")]
                public string Number { get; set; }

                [JsonPropertyName("description")]
                public string Description { get; set; }

                [JsonPropertyName("created_at")]
                public DateTime CreatedAt { get; set; }
            }

            public class ApiRelationships
            {
                [JsonPropertyName("order")]
                public ApiOrder Order { get; set; }

                public class ApiOrder
                {
                    [JsonPropertyName("data")]
                    public ApiOrderData OrderData { get; init; }

                    public class ApiOrderData
                    {
                        [JsonPropertyName("id")]
                        public string OrderId { get; init; }
                    }
                }
            }
        }

        public class ApiIncluded
        {
            [JsonPropertyName("id")]
            public string IncludedOrderId { get; init; }

            [JsonPropertyName("attributes")]
            public ApiOrdersAttributes Attributes { get; set; }

            public class ApiOrdersAttributes
            {
                [JsonPropertyName("reference")]
                public string OrderReference { get; set; }

                [JsonPropertyName("items")]
                public OrdersItems[] OrderItems { get; set; }

                public class OrdersItems
                {
                    [JsonPropertyName("attributes")]
                    public OrdersItemsAttributes OrderItemsAttributes { get; set; }

                    public class OrdersItemsAttributes
                    {
                        [JsonPropertyName("setup_price")]
                        public string OrderSetupPrice { get; set; }

                        [JsonPropertyName("monthly_price")]
                        public string OrderMonthlyPrice { get; set; }

                        // calculated property
                        public decimal SetupPrice => decimal.Parse(this.OrderSetupPrice);

                        public decimal MonthlyPrice => decimal.Parse(this.OrderMonthlyPrice);
                    }
                }
            }
        }
    }
}
