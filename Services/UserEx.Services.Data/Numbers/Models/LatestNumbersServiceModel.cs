namespace UserEx.Services.Data.Numbers.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LatestNumbersServiceModel
    {
        public int Id { get; init; }

        public string DidNumber { get; init; }

        public decimal MonthlyPrice { get; init; }

        public string Description { get; init; }
    }
}
