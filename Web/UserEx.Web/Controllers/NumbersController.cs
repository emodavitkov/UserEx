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

        public IActionResult All(string provider, string searchTerm)
        {
            var numbersQuery = this.data.Numbers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(provider))
            {
                numbersQuery = numbersQuery.Where(n => n.Provider.Name == provider);
            }


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                numbersQuery = numbersQuery.Where(n =>

                  // (n.DidNumber + " " + n.Provider.Name).ToLower().Contains(searchTerm.ToLower()) ||
                   (n.DidNumber).ToLower().Contains(searchTerm.ToLower()) ||
                    n.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            var numbers = numbersQuery
                .OrderByDescending(n => n.Id)
                .Select(n => new NumberListingViewModel
                {
                    Id = n.Id,
                    DidNumber = n.DidNumber,
                    MonthlyPrice = n.MonthlyPrice,
                    Description = n.Description,
                    Provider = n.Provider.Name,
                })
                .ToList();

            // brand
            var numberProviders = this.data
                .Numbers
                .Select(p => p.Provider.Name)
                .Distinct()
                .ToList();

            return this.View(new AllNumbersQueryModel
            {
                Providers = numberProviders,
                Numbers = numbers,
                SearchTerm = searchTerm,
            });

            // return RedirectToAction("Index", "Home");
        }

        public IActionResult Add() => this.View(new AddNumberManualModel
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

            if (!this.ModelState.IsValid)
            {
                number.Providers = this.GetNumberProviders();

                return this.View(number);
            }

            var numberData = new Number
            {
                ProviderId = number.ProviderId,
                DidNumber = number.DidNumber,
                OrderReference = number.OrderReference,
                SetupPrice = number.SetupPrice,
                MonthlyPrice = number.MonthlyPrice,
                Description = number.Description,
                IsActive = number.IsActive,
                StartDate = number.StartDate,
                EndDate = number.EndDate,
            };

            this.data.Numbers.Add(numberData);

            this.data.SaveChanges();

            return this.RedirectToAction(nameof(this.All));

            // return RedirectToAction("Index", "Home");
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
