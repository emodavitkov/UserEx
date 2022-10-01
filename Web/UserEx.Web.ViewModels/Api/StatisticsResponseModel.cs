namespace UserEx.Web.ViewModels.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StatisticsResponseModel
    {
        public int TotalNumbers { get; init; }

        public int TotalUsers { get; init; }

        public int TotalMonthlyCost { get; init; }
    }
}
