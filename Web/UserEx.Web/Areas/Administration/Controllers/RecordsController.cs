using Microsoft.EntityFrameworkCore;

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
    using OfficeOpenXml;
    using UserEx.Data;
    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers;
    using UserEx.Web.ViewModels.Numbers;
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

        // TBD model state validations and provider selection

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadRecord(IFormFile file)
        {
            var bulkRecords = new List<UploadRecordModel>();
            var resultProviderId = 0;

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

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
                        var providerName = worksheet.Cells[row, 7].Value.ToString().ToLower().Trim();

                        resultProviderId = this.GetProviderName(providerName);

                        if (this.data.Providers.FirstOrDefault(p => p.Name.ToLower() == providerName) == null)
                        {
                            continue;
                        }

                        bulkRecords.Add(new UploadRecordModel
                        {
                            Date = Convert.ToDateTime(worksheet.Cells[row, 1].Value),
                            CallerNumber = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            CallingNumber = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            DialCode = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            BuyRate = Convert.ToDecimal(worksheet.Cells[row, 5].Value),
                            Duration = Convert.ToInt32(worksheet.Cells[row, 6].Value),
                            ProviderId = resultProviderId,
                        });
                    }
                }
            }

            var recordsDelete = await this.data.Records.AsQueryable().ToListAsync();
            this.data.Records.RemoveRange(recordsDelete);
           // this.TempData[GlobalMessageKey] = "The OLD CDRs were deleted!";

            // var entities = await Context.UserGroupPainAreas.Where(ug => ug.UserGroupId == userGroupId).ToListAsync();
            // Context.UserGroupPainAreas.RemoveRange(entities);

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

            this.TempData[GlobalMessageKey] = "The previous CDRs were deleted and the new file added!";

            return this.RedirectToAction(nameof(this.UploadRecord));
        }

        public int GetProviderName(string providerName)
            => this.data
                .Providers
                .Where(p => p.Name.ToLower() == providerName)
                .Select(n => n.Id)
                .FirstOrDefault();

        // public List<int> GetProviderName(string providerName)
        //   => this.data
        //       .Providers
        //       .Where(p => p.Name.ToLower() == providerName)
        //       .Select(n => n.Id)
        //       .ToList();
    }
}
