namespace UserEx.Services.Data.Records
{
    using System.Collections.Generic;

    using UserEx.Web.ViewModels.Records;

    public interface IRecordService
    {
        public void Upload(List<UploadRecordModel> bulkRecords);

        public int GetProviderName(string providerName);

        public int GetNumberId(string callerNumber);

        public bool ProviderNameExists(string providerName);
    }
}
