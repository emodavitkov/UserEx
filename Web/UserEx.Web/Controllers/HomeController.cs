namespace UserEx.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using UserEx.Data;
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Statistics;
    using UserEx.Web.ViewModels;
    using UserEx.Web.ViewModels.Home;
    using UserEx.Web.ViewModels.Numbers;

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

            // move to service
            // var latestNumbers = this.numbers
            //    .Latest()
            //    .ToList();

            // var numbers = this.data
            //    .Numbers
            //    .OrderByDescending(n => n.Id)
            //    .Select(n => new NumberIndexViewModel
            //    {
            //        Id = n.Id,
            //        DidNumber = n.DidNumber,
            //        MonthlyPrice = n.MonthlyPrice,
            //        Description = n.Description,
            //    })
            //    .Take(3)
            //    .ToList();
            var totalStatistics = this.statistics.Total();

            return this.View(new IndexViewModel
            {
                TotalNumbers = totalStatistics.TotalNumbers,
                TotalUsers = totalStatistics.TotalUsers,
                Numbers = latestNumbers,
            });

            // return this.View(numbers);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
