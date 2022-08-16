namespace UserEx.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data;
    using UserEx.Data.Models;
    using UserEx.Web.ViewModels.Numbers;

    public class NumbersController : Controller
    {
        private readonly ApplicationDbContext data;

        public NumbersController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult Add() => View(new AddNumberManualModel
        {
            Providers = this.GetNumberProviders(),
        });

        [HttpPost]
        public IActionResult Add(AddNumberManualModel number)
        {
            if (!this.data.Providers.Any(p => p.Id == number.ProviderId))
            {
                this.ModelState.AddModelError(nameof(number.ProviderId), "Provider does not exist.");
            }

            if (!ModelState.IsValid)
            {
                number.Providers = this.GetNumberProviders();

                return View(number);
            }

            var numberData = new Number
            { 
                ProviderId= number.ProviderId,
                DidNumber = number.DidNumber,
                OrderReference = number.OrderReference,
                SetupPrice = number.SetupPrice,
                MonthlyPrice = number.MonthlyPrice,
                Description = number.Description,
                StartDate = number.StartDate,
                EndDate = number.EndDate,
            };

            this.data.Numbers.Add(numberData);

            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<NumberProviderViewModel> GetNumberProviders()
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
