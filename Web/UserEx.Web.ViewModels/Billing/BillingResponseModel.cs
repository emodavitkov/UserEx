namespace UserEx.Web.ViewModels.Billing
{
    using System;
    using System.Collections.Generic;

    using UserEx.Web.ViewModels.Numbers;

    public class BillingResponseModel
    {
        public DateTime StartDate { get; set; } = DateTime.Now;

       public DateTime EndDate { get; set; } = DateTime.Now;

       public int ProviderId { get; set; }

       public IEnumerable<NumberProviderViewModel> Providers { get; set; }
    }
}
