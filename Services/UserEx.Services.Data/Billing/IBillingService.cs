namespace UserEx.Services.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UserEx.Web.ViewModels.Api;
    using UserEx.Web.ViewModels.Numbers;

    using static UserEx.Services.Data.Billing.BillingService;

    public interface IBillingService
    {
        // public void Balance(BalancesApiResponseModel result);
        public Task<BalancesApiResponseModel> GetBalance();

        public decimal CostByProviderId(int providerId);

        public decimal CostAllProviderByDate(DateTime startDate, DateTime endDate);

        public decimal CostProcuredNumbers();

        // public IList<double?> CostCallsByMonthChart();
        public IList<CostSumByMonth> CostCallsByMonthChart();

        public IList<CostNumberProvisionSumByMonth> CostProcuredNumbersByMonthChart();

        // public decimal CostProcuredNumbersByMonthChart();
        IEnumerable<NumberProviderViewModel> AllNumberProviders();

        bool ProviderExists(int providerId);
    }
}
