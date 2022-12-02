namespace UserEx.Services.Data.Numbers.Models
{
    public class NumberServiceModel
    {
        public int Id { get; init; }

        public string DidNumber { get; set; }

        public decimal MonthlyPrice { get; set; }

        public string Description { get; set; }

        public string Provider { get; init; }

        public bool IsPublic { get; init; }
    }
}
