namespace UserEx.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using UserEx.Data;
    using UserEx.Data.Common;
    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers;
    using UserEx.Web.Controllers;
    using UserEx.Web.ViewModels.Numbers;
    using UserEx.Web.ViewModels.Rates;

    using static UserEx.Common.GlobalConstants;

    public class RatesController : AdministrationController
    {
        private readonly ApplicationDbContext data;
        private readonly INumberService numbers;

        public RatesController(
             ApplicationDbContext data, INumberService numbers)
        {
            this.data = data;
            this.numbers = numbers;
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
        public async Task<IActionResult> UploadRate(IFormFile file, int providerId)
        {
            var bulkRates = new List<UploadRateModel>();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

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

            foreach (var rate in bulkRates)
            {
                var numberFromExcel = new Rate()
                {
                    DestinationName = rate.DestinationName,
                    DialCode = rate.DialCode,
                    Cost = rate.Cost,
                    ProviderId = rate.ProviderId,
                };
                this.data.Rates.Add(numberFromExcel);
            }

            this.data.SaveChanges();

            this.TempData[GlobalMessageKey] = "Rates were added!";

            return this.RedirectToAction(nameof(this.UploadRate));
        }
    }
}
