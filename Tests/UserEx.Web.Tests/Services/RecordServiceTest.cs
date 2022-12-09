namespace UserEx.Web.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UserEx.Data.Models;
    using UserEx.Services.Data.Records;
    using UserEx.Web.Tests.Mocks;
    using UserEx.Web.ViewModels.Records;
    using Xunit;

    public class RecordServiceTest
    {
        [Fact]
        public void DoesRecordUploads()
        {
            // Arrange
            var recordUpload = new List<UploadRecordModel>()
            {
                new UploadRecordModel()
                {
                    Date = DateTime.Now,
                    CallerNumber = "123456789",
                    CallingNumber = "123456780",
                    BuyRate = 1M,
                    Duration = 20,
                    ProviderId = 1,
                    DialCode = "123",
                },
                new UploadRecordModel()
                {
                    Date = DateTime.Now,
                    CallerNumber = "123456782",
                    CallingNumber = "123456783",
                    BuyRate = 2M,
                    Duration = 10,
                    ProviderId = 2,
                    DialCode = "1234",
                },
            };

            using var data = DatabaseMock.Instance;

            var recordService = new RecordService(data);

            // Act
            recordService.Upload(recordUpload);

            // Assert
            Assert.Equal(2, data.Records.Count());
        }

        [Fact]
        public void DoesProviderIdReturnedBasedOnName()
        {
            // Arrange
            var providerName = "test";
            var providerId = 1;
            var providersList = new List<Provider>()
            {
                new Provider()
                {
                    Id = providerId,
                    Name = "test",
                },
                new Provider()
                {
                    Id = 2,
                    Name = "test1",
                },
            };

            using var data = DatabaseMock.Instance;

            var recordServiceProviderId = new RecordService(data);

            data.Providers.AddRange(providersList);
            data.SaveChanges();

            // Act
            var result = recordServiceProviderId.GetProviderName(providerName);

            // Assert
            Assert.Equal(providerId, result);
        }

        [Fact]
        public void DoesNumberIdReturnedBasedOnNumber()
        {
            // Arrange
            var didNumber = "0987654320";
            var numberId = 1;

            var numbersList = new List<Number>()
            {
                new Number()
                {
                    Id = numberId,
                    PartnerId = 1,
                    DidNumber = didNumber,
                    Description = "TestDescription",
                    StartDate = DateTime.Now,
                    OrderReference = "test",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 10M,
                    SetupPrice = 10M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 2,
                    PartnerId = 1,
                    DidNumber = "0987654321",
                    Description = "TestDescription2",
                    StartDate = DateTime.Now,
                    OrderReference = "test",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 10M,
                    SetupPrice = 10M,
                    IsPublic = true,
                },
            };

            using var data = DatabaseMock.Instance;

            var numberProviderId = new RecordService(data);

            data.Numbers.AddRange(numbersList);
            data.SaveChanges();

            // Act
            var result = numberProviderId.GetNumberId(didNumber);

            // Assert
            Assert.Equal(numberId, result);
        }

        [Fact]
        public void DoesReturnTrueIfProviderNameExists()
        {
            // Arrange
            var providerName = "test";
            var providerId = 1;
            var providersList = new List<Provider>()
            {
                new Provider()
                {
                    Id = providerId,
                    Name = "test",
                },
                new Provider()
                {
                    Id = 2,
                    Name = "test1",
                },
            };

            using var data = DatabaseMock.Instance;

            var providers = new RecordService(data);

            data.Providers.AddRange(providersList);
            data.SaveChanges();

            // Act
            var result = providers.ProviderNameExists(providerName);

            // Assert
            Assert.True(result);
        }
    }
}
