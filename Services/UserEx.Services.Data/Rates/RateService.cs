namespace UserEx.Services.Data.Rates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data;
    using UserEx.Data.Models;
    using UserEx.Web.ViewModels.Numbers;
    using UserEx.Web.ViewModels.Rates;

    public class RateService : IRateService
    {
        private readonly ApplicationDbContext data;

        public RateService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void Upload(int providerId, List<UploadRateModel> bulkRates)
        {
            var ratesDelete = this.data.Rates.AsQueryable().Where(r => r.ProviderId == providerId).ToList();
            this.data.Rates.RemoveRange(ratesDelete);

            foreach (var rate in bulkRates)
            {
                var numberFromExcel = new Rate()
                {
                    DestinationName = rate.DestinationName,
                    DialCode = rate.DialCode,
                    Cost = rate.Cost,
                    ProviderId = rate.ProviderId,
                };
                this.data.Rates.Add(numberFromExcel);
            }

            this.data.SaveChanges();
        }

        public IEnumerable<NumberProviderViewModel> AllProviders()
        => this.data
            .Providers
            .Select(p => new NumberProviderViewModel()
        {
            Id = p.Id,
            Name = p.Name,
        })
        .ToList();
    }
}
