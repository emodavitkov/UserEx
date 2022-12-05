namespace UserEx.Services.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

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
            var query = this.data
                .Records
                .AsQueryable();

            if (providerId != 0)
            {
                query = query.Where(x => x.ProviderId == providerId);
            }

            var result = query
            .Sum(x => x.BuyRate * x.Duration / 60);

            // var result = this.data
            //    .Records
            //    .Where(p => p.ProviderId == providerId)
            //    /*.Sum(x => x.BuyRate * x.Duration / 60);*/
            return result;
        }

        public decimal CostAllProviderByDate(DateTime startDate, DateTime endDate)
        {
            decimal result = 0;

            try
            {
                result = this.data
                    .Records
                    .Where(d => d.Date.Date >= startDate.Date && d.Date.Date <= endDate.Date)
                    .Sum(x => x.BuyRate * x.Duration / 60);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }

        public decimal CostProcuredNumbers()
        {
            var costResult = new List<decimal>();

            var result = this.data
                .Records
                .Where(n => n.NumberId != null)
                .Select(x => new
                {
                    NumberId = x.NumberId,
                    MonthlyPrice = x.Number.MonthlyPrice,
                })
                .Distinct()
                .Sum(x => x.MonthlyPrice);

            return result;
        }

        // public IList<double?> CostCallsByMonthChart()
        public IList<CostSumByMonth> CostCallsByMonthChart()
        {
            var startDate = DateTime.UtcNow.AddYears(-1); // getting date before one year
            var startYear = startDate.Year;
            var startMonth = startDate.Month;
            startDate = new DateTime(startYear, startMonth, 1); // getting first day of month

            var resultQuery = this.data.Records.Where(r => r.Date >= startDate && r.Duration > 0).AsQueryable();

            var groupedByYearAndMonthResult = resultQuery.GroupBy(r => new { r.Date.Year, r.Date.Month }); // Make records in groups with same Year and Month

            var result = groupedByYearAndMonthResult
                .Select(g =>
                    new CostSumByMonth
                    {
                        Date = g.First().Date, // getting one day from group only for sorting
                        MonthDisplay = $"{g.First().Date:MMM} {g.Key.Year}",
                        CostSum = g.Sum(groupRecords => groupRecords.BuyRate * groupRecords.Duration / 60),
                    })
                .OrderBy(g => g.Date)
                .ToList();

            return result;

            // IList<double?> numbersList = new List<double?>();
            // for (int i = 1; i <= 12; i++)
            // {
            //   var result = this.data
            //        .Records
            //        .Where(x => x.Duration > 0 && x.Date.Month == i)
            //        .Sum(x => x.BuyRate * x.Duration / 60);
            //   double convert = (double)result;
            //   numbersList.Add(convert);
            // }
            // return numbersList;
        }

        public IList<CostNumberProvisionSumByMonth> CostProcuredNumbersByMonthChart()
        {
            // getting date before one year
            var startDate = DateTime.UtcNow.AddYears(-1);
            var startYear = startDate.Year;
            var startMonth = startDate.Month;

            // getting first day of month
            startDate = new DateTime(startYear, startMonth, 1);

            var resultQuery = this.data.Records.Where(r => r.Date >= startDate && r.NumberId != null).AsQueryable();

            // Make records in groups with same Year and Month
            var groupedByYearAndMonthResult = resultQuery.GroupBy(r => new { r.Date.Year, r.Date.Month });

            // var result = this.data
            //    .Records
            //    .Where(n => n.NumberId != null)
            //    .Select(x => new
            //    {
            //        NumberId = x.NumberId,
            //        MonthlyPrice = x.Number.MonthlyPrice,
            //    })
            //    .Distinct()
            //    .Sum(x => x.MonthlyPrice);


            var result = groupedByYearAndMonthResult
                .Select(g =>
                    new CostNumberProvisionSumByMonth
                    {
                        // getting one day from group only for sorting
                        Date = g.First().Date,

                        MonthDisplay = $"{g.First().Date:MMM} {g.Key.Year}",
                        CostSum = g.Sum(groupRecords => groupRecords.Number.MonthlyPrice),
                    })
                .OrderBy(g => g.Date)
                .ToList();

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

        public class CostSumByMonth
        {
            public DateTime Date { get; set; }

            public string MonthDisplay { get; set; }

            public decimal? CostSum { get; set; }
        }

        public class CostNumberProvisionSumByMonth
        {
            public DateTime Date { get; set; }

            public string MonthDisplay { get; set; }

            public decimal? CostSum { get; set; }
        }
    }
}
