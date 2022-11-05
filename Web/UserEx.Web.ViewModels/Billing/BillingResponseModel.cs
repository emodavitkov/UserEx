namespace UserEx.Web.ViewModels.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Web.ViewModels.Numbers;

    public class BillingResponseModel
    {
       // public string BalanceAmount { get; set; }
       public DateTime StartDate { get; set; } = DateTime.Now;

       public DateTime EndDate { get; set; } = DateTime.Now;

       public int ProviderId { get; set; }

       public IEnumerable<NumberProviderViewModel> Providers { get; set; }
    }
}
