namespace UserEx.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data;
    using UserEx.Web.ViewModels;
    using UserEx.Web.ViewModels.Home;
    using UserEx.Web.ViewModels.Numbers;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext data;

        public HomeController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            var totalNumbers = this.data.Numbers.Count();

            var numbers = this.data
                .Numbers
                .OrderByDescending(n => n.Id)
                .Select(n => new NumberIndexViewModel
                {
                    Id = n.Id,
                    DidNumber = n.DidNumber,
                    MonthlyPrice = n.MonthlyPrice,
                    Description = n.Description,
                })
                .Take(3)
                .ToList();


            return this.View(new IndexViewModel
            {
                TotalNumbers = totalNumbers,
                Numbers = numbers,
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
