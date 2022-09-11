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

        public IActionResult All([FromQuery]AllNumbersQueryModel query)
        {
            var numbersQuery = this.data.Numbers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Provider))
            {
                numbersQuery = numbersQuery.Where(n => n.Provider.Name == query.Provider);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                numbersQuery = numbersQuery.Where(n =>

                    // (n.DidNumber + " " + n.Provider.Name).ToLower().Contains(searchTerm.ToLower()) ||
                    n.DidNumber.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    n.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            numbersQuery = query.Sorting switch
            {
                NumberSorting.DateCreated => numbersQuery.OrderByDescending(n => n.StartDate),
                NumberSorting.MonthlyPrice => numbersQuery.OrderByDescending(n => n.MonthlyPrice),
                NumberSorting.Description => numbersQuery.OrderBy(n => n.Description),
                _ => numbersQuery.OrderByDescending(n => n.Id),

                // NumberSorting.Description or _ => numbersQuery.OrderByDescending(n => n.Id),
                // _ => carsQuery.OrderByDescending(c => c.Id)
            };

            // var totalNumbers = this.data.Numbers.Count();
            var totalNumbers = numbersQuery.Count();

            var numbers = numbersQuery
                .Skip((query.CurrentPage - 1) * AllNumbersQueryModel.NumbersPerPage)
                .Take(AllNumbersQueryModel.NumbersPerPage)

                // .OrderByDescending(n => n.Id)
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
                .OrderBy(p => p)
                .ToList();

            query.TotalNumbers = totalNumbers;
            query.Providers = numberProviders;
            query.Numbers = numbers;

            return this.View(query);

            // return this.View(new AllNumbersQueryModel
            // {
            //    Provider = provider,
            //    Providers = numberProviders,
            //    Numbers = numbers,
            //    Sorting = sorting,
            //    SearchTerm = searchTerm,
            // });

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
