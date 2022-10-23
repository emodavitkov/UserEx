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
    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers;
    using UserEx.Web.ViewModels.Rates;
    using UserEx.Web.ViewModels.Records;

    using static UserEx.Common.GlobalConstants;

    public class RecordsController : AdministrationController
    {
        private readonly ApplicationDbContext data;
        private readonly INumberService numbers;

        public RecordsController(
            ApplicationDbContext data,
            INumberService numbers)
        {
            this.data = data;
            this.numbers = numbers;
        }

        [Authorize]
        public IActionResult UploadRecord()
        {
            return this.View(new UploadRecordModel()
            {
                // Providers = this.numbers.AllNumberProviders(),
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadRecord(IFormFile file)
        {
            var bulkRecords = new List<UploadRecordModel>();

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
                        bulkRecords.Add(new UploadRecordModel
                        {
                            Date = Convert.ToDateTime(worksheet.Cells[row, 1].Value),
                            CallerNumber = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            CallingNumber = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            DialCode = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            BuyRate = Convert.ToDecimal(worksheet.Cells[row, 5].Value),
                            Duration = Convert.ToInt32(worksheet.Cells[row, 6].Value),
                            ProviderId = 1,
                        });
                    }
                }
            }

            foreach (var record in bulkRecords)
            {
                var numberFromExcel = new Record
                {
                 Date = record.Date,
                 CallerNumber = record.CallerNumber,
                 CallingNumber = record.CallingNumber,
                 BuyRate = record.BuyRate,
                 Duration = record.Duration,
                 ProviderId = record.ProviderId,
                 DialCode = record.DialCode,
                };
                this.data.Records.Add(numberFromExcel);
            }

            this.data.SaveChanges();

            this.TempData[GlobalMessageKey] = "CDRs were added!";

            return this.RedirectToAction(nameof(this.UploadRecord));
        }
    }
}
