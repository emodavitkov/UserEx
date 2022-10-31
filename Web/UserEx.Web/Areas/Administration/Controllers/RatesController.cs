namespace UserEx.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using OfficeOpenXml;
    using UserEx.Data;
    using UserEx.Data.Common;
    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Rates;
    using UserEx.Web.Controllers;
    using UserEx.Web.ViewModels.Numbers;
    using UserEx.Web.ViewModels.Rates;

    using static UserEx.Common.GlobalConstants;

    public class RatesController : AdministrationController
    {
        private readonly INumberService numbers;
        private readonly IRateService rates;

        public RatesController(
             INumberService numbers,
             IRateService rates)
        {
            this.numbers = numbers;
            this.rates = rates;
        }

        [Authorize]
        public IActionResult UploadRate()
        {
            return this.View(new UploadRateModel
            {
                Providers = this.numbers.AllNumberProviders(),
            });
        }

        // TBD model state validations and provider selection
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadRate(IFormFile file, int providerId, UploadRateModel ratesModel)
        {
            // as input UploadRateModel rates
            var bulkRates = new List<UploadRateModel>();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

            // TBD
            this.ModelState.Remove("DestinationName");
            this.ModelState.Remove("DialCode");

            // moving to services
            if (!this.ModelState.IsValid)
            {
                ratesModel.Providers = this.rates.AllProviders();

                // ratesModel.Providers = this.data
                 //   .Providers
                 //   .Select(p => new NumberProviderViewModel()
                 //   {
                 //       Id = p.Id,
                 //       Name = p.Name,
                 //   })
                 // .ToList();
                return this.View();
            }

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (file == null)
            {
                return this.BadRequest("No file is added!");
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
                        bulkRates.Add(new UploadRateModel
                        {
                            DestinationName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            DialCode = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Cost = Convert.ToDecimal(worksheet.Cells[row, 3].Value),
                            ProviderId = providerId,
                        });
                    }
                }
            }

            // moved to service
            // var ratesDelete = await this.data.Rates.AsQueryable().Where(r => r.ProviderId == providerId).ToListAsync();
            // this.data.Rates.RemoveRange(ratesDelete);

            // var entities = await Context.UserGroupPainAreas.Where(ug => ug.UserGroupId == userGroupId).ToListAsync();
            // Context.UserGroupPainAreas.RemoveRange(entities);

            // if (!this.numbers.ProviderExists(number.ProviderId))
            // {
            //    this.ModelState.AddModelError(nameof(number.ProviderId), "Provider does not exist.");
            // }

            // moved to service
            // foreach (var rate in bulkRates)
            // {
            //    var numberFromExcel = new Rate()
            //    {
            //        DestinationName = rate.DestinationName,
            //        DialCode = rate.DialCode,
            //        Cost = rate.Cost,
            //        ProviderId = rate.ProviderId,
            //    };
            //    this.data.Rates.Add(numberFromExcel);
            // }
            // this.data.SaveChanges();
            this.rates.Upload(providerId, bulkRates);
            this.TempData[GlobalMessageKey] = "The selected Rates are added (old deleted)!";

            return this.RedirectToAction(nameof(this.UploadRate));
        }
    }
}
