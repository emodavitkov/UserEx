
namespace UserEx.Services.Data.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data;
    using UserEx.Services.Data.Numbers.Models;
    using UserEx.Web.ViewModels.Numbers;

    public class NumberService : INumberService
    {
        private readonly ApplicationDbContext data;

        public NumberService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public NumberQueryServiceModel All(
            string provider,
            string searchTerm,
            NumberSorting sorting,
            int currentPage,
            int numbersPerPage)
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
                    n.DidNumber.ToLower().Contains(searchTerm.ToLower()) ||
                    n.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            numbersQuery = sorting switch
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
                .Skip((currentPage - 1) * numbersPerPage)
                .Take(numbersPerPage)

                // .OrderByDescending(n => n.Id)
                .Select(n => new NumberServiceModel()
                {
                    Id = n.Id,
                    DidNumber = n.DidNumber,
                    MonthlyPrice = n.MonthlyPrice,
                    Description = n.Description,
                    Provider = n.Provider.Name,
                })
                .ToList();

            //// brand
            // var numberProviders = this.data
            //    .Numbers
            //    .Select(p => p.Provider.Name)
            //    .Distinct()
            //    .OrderBy(p => p)
            //    .ToList();

            return new NumberQueryServiceModel
            {
                TotalNumbers = totalNumbers,
                CurrentPage = currentPage,
                NumbersPerPage = numbersPerPage,
                Numbers = numbers,
            };
        }

        public IEnumerable<string> AllNumberProviders()
          => this.data
             .Numbers
             .Select(p => p.Provider.Name)
             .Distinct()
             .OrderBy(p => p)
             .ToList();
    }
}
