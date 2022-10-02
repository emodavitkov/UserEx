namespace UserEx.Services.Data.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StatisticsServiceModel
    {
        public int TotalNumbers { get; init; }

        public int TotalUsers { get; set; }

        public int TotalMonthlyCost { get; set; }
    }
}
