namespace UserEx.Services.Data.Numbers.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data.Models;

    public class NumberServiceModel
    {
        public int Id { get; init; }

        public string DidNumber { get; set; }

        public decimal MonthlyPrice { get; set; }

        public string Description { get; set; }

        public string Provider { get; init; }
    }
}
