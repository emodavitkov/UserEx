namespace UserEx.Services.Data.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StatisticsServiceModel
    {
        public int TotalNumbersApproved { get; init; }

        public int TotalUsers { get; set; }

        public int TotalNumbersNotApproved { get; set; }
    }
}
