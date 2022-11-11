namespace UserEx.Services.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using UserEx.Data;
    using UserEx.Web.ViewModels.Api;
    using UserEx.Web.ViewModels.Numbers;

    public class BillingService : IBillingService
    {
        private readonly IConfiguration config;
        private readonly ApplicationDbContext data;

        public BillingService(IConfiguration config, ApplicationDbContext data)
        {
            this.config = config;
            this.data = data;
        }

        public async Task<BalancesApiResponseModel> GetBalance()
        {
            var didlogicApiKey = this.config["Didlogic:ApiKey"];

            var balance = new BalancesApiResponseModel { };

            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage =
                    new HttpRequestMessage(HttpMethod.Get,
                        $"https://didlogic.com/api/v1/balance.json?apiid={@didlogicApiKey}")
                    {
                        Headers =
                        {
                            { "Host", "didlogic.com" },
                        },
                    };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                var result = string.Empty;

                using (HttpContent content = httpResponseMessage.Content)
                {
                    result = content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                result = result.Remove(0, 11).TrimEnd('}');

                balance = new BalancesApiResponseModel
                {
                    BalanceAmount = result,
                };
            }

            return balance;
        }

        public decimal CostByProviderId(int providerId)
        {
            var result = this.data
                .Records
                .Where(p => p.ProviderId == providerId)
                .Sum(x => x.BuyRate * x.Duration / 60);

            return result;
        }

        public decimal CostAllProviderByDate(DateTime startDate, DateTime endDate)
        {
            var result = this.data
                .Records
                .Where(d => d.Date.Date >= startDate.Date && d.Date.Date <= endDate.Date)
                .Sum(x => x.BuyRate * x.Duration / 60);

            return result;
        }

        public decimal CostProcuredNumbers()
        {
            var costResult = new List<decimal>();


            var result = this.data
                .Records
                .Where(n => n.NumberId != null)
                .Take(1)
                .Sum(x => x.Number.MonthlyPrice);

            return result;
        }

        public IEnumerable<NumberProviderViewModel> AllNumberProviders()
           => this.data
               .Providers
               .Select(p => new NumberProviderViewModel()
               {
                   Id = p.Id,
                   Name = p.Name,
               })
               .ToList();

        public bool ProviderExists(int providerId)
            => this.data
                .Providers
                .Any(p => p.Id == providerId);
    }
}
