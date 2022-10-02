using UserEx.Services.Data.Numbers;
using UserEx.Services.Data.Numbers.Models;

namespace UserEx.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data;
    using UserEx.Data.Common;
    using UserEx.Data.Models;
    using UserEx.Web.ViewModels.Numbers;

    public class NumbersController : Controller
    {
        private readonly INumberService numbers;
        private readonly ApplicationDbContext data;

        public NumbersController(ApplicationDbContext data, INumberService numbers)
        {
            this.data = data;
            this.numbers = numbers;
        }

        public IActionResult All([FromQuery]AllNumbersQueryModel query)
        {
            // move to service

            var queryResult = this.numbers.All(
                query.Provider,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllNumbersQueryModel.NumbersPerPage);

            // var numbersQuery = this.data.Numbers.AsQueryable();

            // if (!string.IsNullOrWhiteSpace(query.Provider))
            // {
            //    numbersQuery = numbersQuery.Where(n => n.Provider.Name == query.Provider);
            // }

            // if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            // {
            //    numbersQuery = numbersQuery.Where(n =>

            //        // (n.DidNumber + " " + n.Provider.Name).ToLower().Contains(searchTerm.ToLower()) ||
            //        n.DidNumber.ToLower().Contains(query.SearchTerm.ToLower()) ||
            //        n.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            //}

            //numbersQuery = query.Sorting switch
            //{
            //    NumberSorting.DateCreated => numbersQuery.OrderByDescending(n => n.StartDate),
            //    NumberSorting.MonthlyPrice => numbersQuery.OrderByDescending(n => n.MonthlyPrice),
            //    NumberSorting.Description => numbersQuery.OrderBy(n => n.Description),
            //    _ => numbersQuery.OrderByDescending(n => n.Id),

            //    // NumberSorting.Description or _ => numbersQuery.OrderByDescending(n => n.Id),
            //    // _ => carsQuery.OrderByDescending(c => c.Id)
            //};

            //// var totalNumbers = this.data.Numbers.Count();
            //var totalNumbers = numbersQuery.Count();

            //var numbers = numbersQuery
            //    .Skip((query.CurrentPage - 1) * AllNumbersQueryModel.NumbersPerPage)
            //    .Take(AllNumbersQueryModel.NumbersPerPage)

            //    // .OrderByDescending(n => n.Id)
            //    .Select(n => new NumberListingViewModel
            //    {
            //        Id = n.Id,
            //        DidNumber = n.DidNumber,
            //        MonthlyPrice = n.MonthlyPrice,
            //        Description = n.Description,
            //        Provider = n.Provider.Name,
            //    })
            //    .ToList();

            // brand move to service
            var numberProviders = this.numbers.AllNumberProviders();

            // var numberProviders = this.data
            //    .Numbers
            //    .Select(p => p.Provider.Name)
            //    .Distinct()
            //    .OrderBy(p => p)
            //    .ToList();

            query.TotalNumbers = queryResult.TotalNumbers;
            query.Providers = numberProviders;

            // query.Numbers = queryResult.Numbers;

            query.Numbers = queryResult.Numbers.Select(n => new NumberListingViewModel
            {
                Id = n.Id,
                Provider = n.Provider,
                DidNumber = n.DidNumber,
                MonthlyPrice = n.MonthlyPrice,
                Description = n.Description,
            });

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

        [Authorize]
        public IActionResult Add()
        {
            if (!this.UserIsPartner())
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

            return this.View(new AddNumberManualModel
            {
                Providers = this.GetNumberProviders(),
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddNumberManualModel number)
        {
            var partnerId = this.data
                .Partners
                .Where(p => p.UserId == this.User.GetId())
                .Select(p => p.Id)
                .FirstOrDefault();

            if (partnerId == 0)
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

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
                Source = number.Source,
                StartDate = number.StartDate,
                EndDate = number.EndDate,
                PartnerId = partnerId,
            };

            this.data.Numbers.Add(numberData);

            this.data.SaveChanges();

            return this.RedirectToAction(nameof(this.All));

            // return RedirectToAction("Index", "Home");
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // var filePath = Path.GetTempFileName(); // Full path to file in temp location
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

           // create folder if not exist
            if (!Directory.Exists(filePath))
            {
               Directory.CreateDirectory(filePath);
            }

            // get file extension
            // FileInfo fileInfo = new FileInfo(file.FileName);
            string fileName = file.FileName;

            string fileNameWithPath = Path.Combine(filePath, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                    await file.CopyToAsync(stream);
            }

            // Copy files to FileSystem using Streams

            // var bytes = file.Sum(f => f.Length);

            // return Ok(new { count = file.Count, bytes, filePath });
            return this.RedirectToAction(nameof(this.All));
        }


        private bool UserIsPartner()
            => this.data
                .Partners
                .Any(p => p.UserId == this.User.GetId());

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
