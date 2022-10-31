namespace UserEx.Services.Data.Rates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UserEx.Web.ViewModels.Numbers;
    using UserEx.Web.ViewModels.Rates;

    public interface IRateService
    {
        public void Upload(int providerId, List<UploadRateModel> bulkRates);

        IEnumerable<NumberProviderViewModel> AllProviders();
    }
}
