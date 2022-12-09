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
    using UserEx.Services.Data.Records;
    using UserEx.Web.ViewModels.Records;

    using static UserEx.Common.GlobalConstants;

    public class RecordsController : AdministrationController
    {
        private readonly IRecordService records;

        public RecordsController(IRecordService records)
        {
            this.records = records;
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

                try
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var firstRowTemplate = "Date*CallerID*Phone Number*Dial Code*Buy Rate*Duration*Provider";
                        var numberOfColumns = 7;
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
                            return this.BadRequest(error: "Wrong CDRs template was used. Re-check and try again!");
                        }

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var providerName = worksheet.Cells[row, 7].Value.ToString().ToLower().Trim();

                            var resultProviderId = this.records.GetProviderName(providerName);

                            if (!this.records.ProviderNameExists(providerName))
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
                catch (Exception)
                {
                    this.TempData[GlobalMessageKey] = "Wrong CDRs file format was used! Try again but with the correct file extension.";
                    return this.RedirectToAction(nameof(this.UploadRecord));
                }
            }

            this.records.Upload(bulkRecords);

            this.TempData[GlobalMessageKey] = "The previous CDRs were deleted and the new file added!";

            return this.RedirectToAction(nameof(this.UploadRecord));
        }
    }
}
