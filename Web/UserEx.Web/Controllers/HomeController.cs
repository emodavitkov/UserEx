namespace UserEx.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Statistics;
    using UserEx.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly INumberService numbers;
        private readonly IStatisticsService statistics;
        private readonly IMemoryCache cache;

        public HomeController(
            IStatisticsService statistics,
            INumberService numbers,
            IMemoryCache cache)
        {
            this.statistics = statistics;
            this.numbers = numbers;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            // adding cache
            const string latestNumbersCacheKey = "LatestNumbersCacheKey";

            var latestNumbers = this.cache.Get<List<NumberIndexViewModel>>(latestNumbersCacheKey);

            if (latestNumbers == null)
            {
                latestNumbers = this.numbers
                    .Latest()
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(latestNumbersCacheKey, latestNumbers, cacheOptions);
            }

            var totalStatistics = this.statistics.Total();

            return this.View(new IndexViewModel
            {
                TotalNumbers = totalStatistics.TotalNumbersApproved,
                TotalUsers = totalStatistics.TotalUsers,
                Numbers = latestNumbers,
            });
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Error() => this.View();
    }
}
