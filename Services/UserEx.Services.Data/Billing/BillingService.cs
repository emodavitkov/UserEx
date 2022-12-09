namespace UserEx.Services.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using UserEx.Data;
    using UserEx.Data.Models;
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

        public IList<CostSumByMonth> CostCallsByMonthChart()
        {
            // getting date before one year
            var startDate = DateTime.UtcNow.AddYears(-1);
            var startYear = startDate.Year;
            var startMonth = startDate.Month;

            // getting first day of month
            startDate = new DateTime(startYear, startMonth, 1);

            var resultQuery = this.data.Records.Where(r => r.Date >= startDate && r.Duration > 0).AsQueryable();

            // Make records in groups with same Year and Month
            var groupedByYearAndMonthResult = resultQuery.GroupBy(r => new { r.Date.Year, r.Date.Month });

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
        }

        public IList<CostNumberProvisionSumByMonth> CostProcuredNumbersByMonthChart()
        {
            // getting date before one year
            var startDate = DateTime.UtcNow.AddYears(-1);
            var startYear = startDate.Year;
            var startMonth = startDate.Month;

            // getting first day of month
            startDate = new DateTime(startYear, startMonth, 1);

            var resultQuery = this.data.Records.Where(r => r.Date >= startDate && r.NumberId != null)
                .Include(x => x.Number)
                .ToList()
                .Distinct(new RecordSameNumberComparer());

                // Make records in groups with same Year and Month
            var groupedByYearAndMonthResult = resultQuery.GroupBy(r => new { r.Date.Year, r.Date.Month });

            var result = groupedByYearAndMonthResult
                .Select(g =>
                    new CostNumberProvisionSumByMonth
                    {
                        // getting one day from group only for sorting
                        Date = g.First().Date,

                        MonthDisplay = $"{g.First().Date:MMM} {g.Key.Year}",
                        CostSum = g
                            .Sum(groupRecords => groupRecords.Number.MonthlyPrice),
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

        public class RecordSameNumberComparer : IEqualityComparer<Record>
        {
            public bool Equals(Record x, Record y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                // Check whether any of the compared objects is null.
                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                // Check whether the numberID properties are equal.
                return x.NumberId == y.NumberId;
            }

            public int GetHashCode(Record record)
            {
                // Get hash code for the Name field if it is not null.
                int hashProductName = record.NumberId == null ? 0 : record.NumberId.GetHashCode();

                // Get hash code for the Code field.
                int hashProductCode = record.NumberId.GetHashCode();

                // Calculate the hash code for the product.
                return hashProductName ^ hashProductCode;
            }
        }
    }
}
