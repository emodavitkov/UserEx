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
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Rates;
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadRate(IFormFile file, int providerId, UploadRateModel ratesModel)
        {
            var bulkRates = new List<UploadRateModel>();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

            this.ModelState.Remove("DestinationName");
            this.ModelState.Remove("DialCode");

            if (!this.ModelState.IsValid)
            {
                ratesModel.Providers = this.rates.AllProviders();

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

                try
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var firstRowTemplate = "Destination Name*Dial Code*Cost";
                        var numberOfColumns = 3;
                        var firstRow = 1;
                        var firstRowList = new List<string>();

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 1; row <= firstRow; row++)
                        {
                            for (int col = 1; col <= numberOfColumns; col++)
                            {
                                var value = Convert.ToString(worksheet.Cells[row, col].Value);
                                firstRowList.Add(value);
                            }
                        }

                        var result = string.Join("*", firstRowList);

                        if (result != firstRowTemplate)
                        {
                            return this.BadRequest(error: "Wrong Rates template was used. Re-check and try again!");
                        }

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
                catch (Exception)
                {
                    this.TempData[GlobalMessageKey] = "Wrong Rates file format was used! Try again but with the correct file extension.";
                    return this.RedirectToAction(nameof(this.UploadRate));
                }
            }

            this.rates.Upload(providerId, bulkRates);
            this.TempData[GlobalMessageKey] = "The selected Rates are added (old deleted)!";

            return this.RedirectToAction(nameof(this.UploadRate));
        }
    }
}
