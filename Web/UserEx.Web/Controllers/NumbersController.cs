using OfficeOpenXml;

namespace UserEx.Web.Controllers
{
    using System;
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
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Numbers.Models;
    using UserEx.Services.Data.Partners;
    using UserEx.Web.ViewModels.Numbers;

    using static UserEx.Common.GlobalConstants;

    public class NumbersController : Controller
    {
        // data to be removed - added for Upload test
        private readonly INumberService numbers;
        private readonly IPartnerService partners;
        private readonly ApplicationDbContext data;

        public NumbersController(
            INumberService numbers,
            IPartnerService partners,
            ApplicationDbContext data)
        {
            this.data = data;
            this.numbers = numbers;
            this.partners = partners;
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

            // // (n.DidNumber + " " + n.Provider.Name).ToLower().Contains(searchTerm.ToLower()) ||
            //        n.DidNumber.ToLower().Contains(query.SearchTerm.ToLower()) ||
            //        n.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            // }

            // numbersQuery = query.Sorting switch
            // {
            //    NumberSorting.DateCreated => numbersQuery.OrderByDescending(n => n.StartDate),
            //    NumberSorting.MonthlyPrice => numbersQuery.OrderByDescending(n => n.MonthlyPrice),
            //    NumberSorting.Description => numbersQuery.OrderBy(n => n.Description),
            //    _ => numbersQuery.OrderByDescending(n => n.Id),

            // // NumberSorting.Description or _ => numbersQuery.OrderByDescending(n => n.Id),
            //    // _ => carsQuery.OrderByDescending(c => c.Id)
            // };

            //// var totalNumbers = this.data.Numbers.Count();
            // var totalNumbers = numbersQuery.Count();

            // var numbers = numbersQuery
            //    .Skip((query.CurrentPage - 1) * AllNumbersQueryModel.NumbersPerPage)
            //    .Take(AllNumbersQueryModel.NumbersPerPage)

            // // .OrderByDescending(n => n.Id)
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
           // var numberProviders = this.numbers.AllNumberProviders();
            var numberProviders = this.numbers.AllNumbersByProvider();

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
        public IActionResult OfficeDids()
        {
            var officeNumbers = this.numbers.ByUser(this.User.GetId());

            return this.View(officeNumbers);
        }

        public IActionResult Details(int id, string information)
        {
            var number = this.numbers.Details(id);

            if (!information.Contains(number.Provider) || !information.Contains(number.DidNumber))
            {
                return this.BadRequest();
            }

            return this.View(number);
        }

        [Authorize]
        public IActionResult Add()
        {
            // if (!this.UserIsPartner())
                if (!this.partners.IsPartner(this.User.GetId()))
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

                return this.View(new NumberManualModel
            {
                // Providers = this.GetNumberProviders(),
                Providers = this.numbers.AllNumberProviders(),
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(NumberManualModel number)
        {
            var partnerId = this.partners.GetIdByUser(this.User.GetId());

            // moved to service GetIdByUser
            // var partnerId = this.data
            //    .Partners
            //    .Where(p => p.UserId == this.User.GetId())
            //    .Select(p => p.Id)
            //    .FirstOrDefault();
            if (partnerId == 0)
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

            if (!this.numbers.ProviderExists(number.ProviderId))
            {
                this.ModelState.AddModelError(nameof(number.ProviderId), "Provider does not exist.");
            }

            // moving to service
            // if (!this.data.Providers.Any(p => p.Id == number.ProviderId))
            // {
            //    this.ModelState.AddModelError(nameof(number.ProviderId), "Provider does not exist.");
            // }
            if (!this.ModelState.IsValid)
            {
                // number.Providers = this.GetNumberProviders();
                number.Providers = this.numbers.AllNumberProviders();

                return this.View(number);
            }

            var numberId = this.numbers.Create(
                number.ProviderId,
                number.DidNumber,
                number.OrderReference,
                number.SetupPrice,
                number.MonthlyPrice,
                number.Description,
                number.IsActive,
                number.Source,
                number.StartDate,
                number.EndDate,
                partnerId);

            // moving to service
            // var numberData = new Number
            // {
            //    ProviderId = number.ProviderId,
            //    DidNumber = number.DidNumber,
            //    OrderReference = number.OrderReference,
            //    SetupPrice = number.SetupPrice,
            //    MonthlyPrice = number.MonthlyPrice,
            //    Description = number.Description,
            //    IsActive = number.IsActive,
            //    Source = number.Source,
            //    StartDate = number.StartDate,
            //    EndDate = number.EndDate,
            //    PartnerId = partnerId,
            // };
            // this.data.Numbers.Add(numberData);
            // this.data.SaveChanges();
            this.TempData[GlobalMessageKey] = "Number added successfully and awaiting for approval!";

            // bug to be fixed later
            // return this.RedirectToAction(nameof(Details), new { id = numberId, information = @number.DidNumber });
            return this.RedirectToAction(nameof(this.All));

            // return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();

            if (!this.partners.IsPartner(userId) && !User.IsAdmin())
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

            var number = this.numbers.Details(id);

            if (number.UserId != userId && !User.IsAdmin())
            {
                return this.Unauthorized();
            }

            return this.View(new NumberManualModel
            {
                DidNumber = number.DidNumber,
                OrderReference = number.OrderReference,
                SetupPrice = number.SetupPrice,
                MonthlyPrice = number.MonthlyPrice,
                Description = number.Description,
                IsActive = number.IsActive,
                Source = SourceEnum.Manual,
                StartDate = number.StartDate,
                EndDate = null,
                ProviderId = number.ProviderId,
                Providers = this.numbers.AllNumberProviders(),
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, NumberManualModel number)
        {
            var partnerId = this.partners.GetIdByUser(this.User.GetId());

            if (partnerId == 0 && !User.IsAdmin())
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

            if (!this.numbers.ProviderExists(number.ProviderId))
            {
                this.ModelState.AddModelError(nameof(number.ProviderId), "Provider does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                number.Providers = this.numbers.AllNumberProviders();
                return this.View(number);
            }

            if (!this.numbers.NumberIsByPartner(id, partnerId) && !User.IsAdmin())
            {
                return this.BadRequest();
            }

            var edited = this.numbers.Edit(
                id,
                number.ProviderId,
                number.DidNumber,
                number.OrderReference,
                number.SetupPrice,
                number.MonthlyPrice,
                number.Description,
                number.IsActive,
                number.Source,
                number.StartDate,
                number.EndDate,
                this.User.IsAdmin());

            if (!edited)
            {
                return this.BadRequest();
            }

            // partnerId
            this.TempData[GlobalMessageKey] = $"Number edited {(this.User.IsAdmin() ? String.Empty : " and awaiting for approval")}!";

            return this.RedirectToAction(nameof(this.All));
        }

        // [Route("[controller]/Upload")]
        [Authorize]
        public IActionResult Upload()
        {
            // if (!this.UserIsPartner())
            if (!this.partners.IsPartner(this.User.GetId()))
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

            return this.View(new NumberManualModel
            {
                // Providers = this.GetNumberProviders(),
                Providers = this.numbers.AllNumberProviders(),
            });
        }

        // [HttpPost("Upload")]
        [HttpPost]
        [Authorize]
        //public async Task<List<NumberManualModel>> Upload(IFormFile file)
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var partnerId = this.partners.GetIdByUser(this.User.GetId());

            var bulkDids = new List<NumberManualModel>();

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
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            bulkDids.Add(new NumberManualModel
                            {
                                    DidNumber = worksheet.Cells[row,1].Value.ToString().Trim(),
                                    OrderReference = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    SetupPrice = (decimal)Convert.ToInt32(worksheet.Cells[row, 3].Value),
                                    MonthlyPrice = (decimal)Convert.ToInt32(worksheet.Cells[row, 4].Value),

                                    // MonthlyPrice = (decimal)worksheet.Cells[row, 4].Value,
                                    Description = worksheet.Cells[row, 5].Value.ToString().Trim(),

                                    // IsActive = (bool)worksheet.Cells[row, 5].Value,
                                    IsActive = true,

                                    // StartDate = (DateTime)worksheet.Cells[row, 6].Value,
                                    StartDate = DateTime.Today,

                                    // EndDate = (DateTime)worksheet.Cells[row, 7].Value,
                                    EndDate = null,
                                    ProviderId = (int)Convert.ToInt32(worksheet.Cells[row, 9].Value),
                                    Source = (SourceEnum)0,

                                    // ProviderId = (int)worksheet.Cells[row, 8].Value,
                                    // Source = (SourceEnum)worksheet.Cells[row, 9].Value,
                            });
                        }
                    }
            }

            // Copy files to FileSystem using Streams

            // var bytes = file.Sum(f => f.Length);

            // return Ok(new { count = file.Count, bytes, filePath });

            // return this.RedirectToAction(nameof(this.All));

            // to add try catch Model validation and move to services 
            foreach (var number in bulkDids)
            {
                var numberFromExcel = new Number
                {
                    DidNumber = number.DidNumber,
                    Description = number.Description,
                    StartDate = number.StartDate,
                    EndDate = number.EndDate,
                    ProviderId = (int)number.ProviderId,
                    Source = number.Source,
                    IsActive = true,
                    IsPublic = false,
                    MonthlyPrice = number.MonthlyPrice,
                    SetupPrice = number.SetupPrice,
                    PartnerId = partnerId,
                };
                this.data.Numbers.Add(numberFromExcel);
            }

            this.data.SaveChanges();

            this.TempData[GlobalMessageKey] = "Bulk numbers were added successfully and awaiting for approval!";

            // return bulkDids;

            // bug to be fixed later
            // return this.RedirectToAction(nameof(Details), new { id = numberId, information = @number.DidNumber });
            return this.RedirectToAction(nameof(this.OfficeDids));
        }

        // [HttpPost("Upload")]
        // public async Task<IActionResult> Upload(IFormFile file)
        // {
        //    // var filePath = Path.GetTempFileName(); // Full path to file in temp location
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
        //    // create folder if not exist
        //    if (!Directory.Exists(filePath))
        //    {
        //        Directory.CreateDirectory(filePath);
        //    }
        //    // get file extension
        //    // FileInfo fileInfo = new FileInfo(file.FileName);
        //    string fileName = file.FileName;
        //    string fileNameWithPath = Path.Combine(filePath, fileName);
        //    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    // Copy files to FileSystem using Streams
        //    // var bytes = file.Sum(f => f.Length);
        //    // return Ok(new { count = file.Count, bytes, filePath });
        //    return this.RedirectToAction(nameof(this.All));
        // }


        // moved in the number service
        // private bool UserIsPartner()
        //    => this.data
        //        .Partners
        //        .Any(p => p.UserId == this.User.GetId());

        // private IEnumerable<NumberProviderViewModel> GetNumberProviders()
        //    => this.data
        //        .Providers
        //        .Select(p => new NumberProviderViewModel()
        //        {
        //            Id = p.Id,
        //            Name = p.Name,
        //        })
        //        .ToList();
    }
}
