namespace UserEx.Web.ViewModels.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BillingResultDataModel
    {
        public decimal SumCostByProvider { get; set; }

        public decimal SumCostByDate { get; set; }
    }
}
