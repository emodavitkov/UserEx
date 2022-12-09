namespace UserEx.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using UserEx.Data.Common;
    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Partners;
    using UserEx.Web.ViewModels.Numbers;

    using static UserEx.Common.GlobalConstants;

    public class NumbersController : Controller
    {
        private readonly INumberService numbers;
        private readonly IPartnerService partners;

        public NumbersController(
            INumberService numbers,
            IPartnerService partners)
        {
            this.numbers = numbers;
            this.partners = partners;
        }

        public IActionResult All([FromQuery]AllNumbersQueryModel query)
        {
            var queryResult = this.numbers.All(
                query.Provider,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllNumbersQueryModel.NumbersPerPage,
                publicOnly: true);

            var numberProviders = this.numbers.AllNumbersByProvider();

            query.TotalNumbers = queryResult.TotalNumbers;
            query.Providers = numberProviders;

            query.Numbers = queryResult.Numbers.Select(n => new NumberListingViewModel
            {
                Id = n.Id,
                Provider = n.Provider,
                DidNumber = n.DidNumber,
                MonthlyPrice = n.MonthlyPrice,
                Description = n.Description,
            });

            return this.View(query);
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

            if (!information.Contains(number.DidNumber))
            {
                return this.BadRequest();
            }

            return this.View(number);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.partners.IsPartner(this.User.GetId()))
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

            return this.View(new NumberManualModel
            {
                Providers = this.numbers.AllNumberProviders(),
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(NumberManualModel number)
        {
            var partnerId = this.partners.GetIdByUser(this.User.GetId());

            if (partnerId == 0)
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

            var result = this.numbers.Create(
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

            if (result > 0)
            {
                this.TempData[GlobalMessageKey] = "Number added successfully and awaiting for approval!";
            }
            else
            {
                this.TempData[GlobalMessageKey] = "Number already exists!";
                number.Providers = this.numbers.AllNumberProviders();
                return this.View(number);
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();

            if (!this.partners.IsPartner(userId) && !this.User.IsAdmin())
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

            var number = this.numbers.Details(id);

            if (number.UserId != userId && !this.User.IsAdmin())
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
            var partnerId = 0;

            if (!this.User.IsAdmin())
            {
                partnerId = this.partners.GetIdByUser(this.User.GetId());
            }

            if (partnerId == 0 && !this.User.IsAdmin())
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

            if (!this.numbers.NumberIsByPartner(id, partnerId) && !this.User.IsAdmin())
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

            this.TempData[GlobalMessageKey] = $"Number edited {(this.User.IsAdmin() ? String.Empty : " and awaiting for approval")}!";

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.GetId();

            if (!this.partners.IsPartner(userId) && !this.User.IsAdmin())
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
                EndDate = DateTime.Today,
                ProviderId = number.ProviderId,
                Providers = this.numbers.AllNumberProviders(),
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id, NumberManualModel number)
        {
            var partnerId = 0;

            if (!this.User.IsAdmin())
            {
                partnerId = this.partners.GetIdByUser(this.User.GetId());
            }

            if (partnerId == 0 && !this.User.IsAdmin())
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

            if (!this.numbers.NumberIsByPartner(id, partnerId) && !this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            var delete = this.numbers.Delete(
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

            if (delete == 0)
            {
                return this.BadRequest();
            }

            this.TempData[GlobalMessageKey] = $"Number is deleted";

            if (this.User.IsAdmin())
            {
                return this.RedirectToAction(nameof(this.All));
            }

            return this.RedirectToAction(nameof(this.OfficeDids));
        }

        [Authorize]
        public IActionResult Upload()
        {
            if (!this.partners.IsPartner(this.User.GetId()))
            {
                return this.RedirectToAction(nameof(PartnersController.SetUp), "Partners");
            }

            return this.View(new NumberManualModel
            {
                Providers = this.numbers.AllNumberProviders(),
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file is null)
            {
                this.ModelState.AddModelError(nameof(file), "Please select file.");
                return this.View();
            }

            var partnerId = this.partners.GetIdByUser(this.User.GetId());

            var bulkDids = new List<NumberManualModel>();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

           // create folder if not exist
            if (!Directory.Exists(filePath))
            {
               Directory.CreateDirectory(filePath);
            }

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
                                    SetupPrice = Convert.ToDecimal(worksheet.Cells[row, 3].Value),
                                    MonthlyPrice = Convert.ToDecimal(worksheet.Cells[row, 4].Value),
                                    Description = worksheet.Cells[row, 5].Value.ToString().Trim(),
                                    IsActive = true,
                                    StartDate = DateTime.Today,
                                    EndDate = null,
                                    ProviderId = (int)Convert.ToInt32(worksheet.Cells[row, 9].Value),
                                    Source = (SourceEnum)0,
                            });
                        }
                    }
            }

            var result = this.numbers.BulkCreate(bulkDids, partnerId);

            if (result == 0)
            {
                this.TempData[GlobalMessageKey] = "No new number were added due to duplication!";
            }
            else
            {
                this.TempData[GlobalMessageKey] = $" {result} numbers (from bulk) were added successfully and awaiting for approval!";
            }

            return this.RedirectToAction(nameof(this.OfficeDids));
        }
    }
}
