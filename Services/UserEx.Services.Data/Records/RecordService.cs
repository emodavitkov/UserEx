namespace UserEx.Services.Data.Records
{
    using System.Collections.Generic;
    using System.Linq;

    using UserEx.Data;
    using UserEx.Data.Models;
    using UserEx.Web.ViewModels.Records;

    public class RecordService : IRecordService
    {
        private readonly ApplicationDbContext data;

        public RecordService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void Upload(List<UploadRecordModel> bulkRecords)
        {
            var recordsDelete = this.data.Records.AsQueryable().ToList();
            this.data.Records.RemoveRange(recordsDelete);

            foreach (var record in bulkRecords)
            {
                var callerNumber = record.CallerNumber;
                int? resultNumberId = null;
                string resultCallerNumberNotProcured = null;

                if (this.GetNumberId(callerNumber) != 0)
                {
                    resultNumberId = this.GetNumberId(callerNumber);
                }
                else
                {
                    resultCallerNumberNotProcured = callerNumber;
                }

                var numberFromExcel = new Record
                {
                    Date = record.Date,
                    CallerNumberNotProcured = resultCallerNumberNotProcured,
                    CallingNumber = record.CallingNumber,
                    BuyRate = record.BuyRate,
                    Duration = record.Duration,
                    ProviderId = record.ProviderId,
                    DialCode = record.DialCode,
                    NumberId = resultNumberId,
                };
                this.data.Records.Add(numberFromExcel);
            }

            this.data.SaveChanges();
        }

        public int GetProviderName(string providerName)
           => this.data
               .Providers
               .Where(p => p.Name.ToLower() == providerName)
               .Select(p => p.Id)
               .FirstOrDefault();

        public int GetNumberId(string callerNumber)
            => this.data
                .Numbers
                .Where(n => n.DidNumber == callerNumber)
                .Select(n => n.Id)
                .FirstOrDefault();

        public bool ProviderNameExists(string providerName)
        {
            var provider = this.data.Providers.FirstOrDefault(p => p.Name == providerName);
            return provider != null;
        }
    }
}
