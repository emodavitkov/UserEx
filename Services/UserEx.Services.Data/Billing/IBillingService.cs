namespace UserEx.Services.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using UserEx.Web.ViewModels.Api;
    using UserEx.Web.ViewModels.Numbers;

    public interface IBillingService
    {
        // public void Balance(BalancesApiResponseModel result);
        public Task<BalancesApiResponseModel> GetBalance();

        public decimal CostByProviderId(int providerId);

        public decimal CostAllProviderByDate(DateTime startDate, DateTime endDate);

        public decimal CostProcuredNumbers();

        IEnumerable<NumberProviderViewModel> AllNumberProviders();

        bool ProviderExists(int providerId);
    }
}
