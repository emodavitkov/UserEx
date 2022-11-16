namespace UserEx.Web.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using UserEx.Data.Models;
    using UserEx.Services.Data.Billing;
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Partners;
    using UserEx.Services.Data.Rates;
    using UserEx.Web.Tests.Mocks;
    using UserEx.Web.ViewModels.Numbers;
    using UserEx.Web.ViewModels.Partners;
    using UserEx.Web.ViewModels.Rates;
    using Xunit;

    public class BillingServiceTest
    {
        [Fact]
        public void DoesReturnCostByGivenProviderId()
        {
            // Arrange
            var startDate = new DateTime(2022, 5, 1);
            var endDate = new DateTime(2022, 5, 31);
            var buyRate = 1M;
            var duration = 60;
            var providerId = 1;

            var myConfiguration = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Nested:Key1", "NestedValue1" },
                { "Nested:Key2", "NestedValue2" },
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            var records = new List<UserEx.Data.Models.Record>()
            {
                new UserEx.Data.Models.Record()
                {
                    Date = new DateTime(2022, 5, 10),
                    CallerNumberNotProcured = "123456789",
                    CallingNumber = "123456780",
                    BuyRate = buyRate,
                    Duration = duration,
                    ProviderId = providerId,
                    DialCode = "123",
                },
                new UserEx.Data.Models.Record()
                {
                    Date = new DateTime(2022, 6, 10),
                    CallerNumberNotProcured = "123456782",
                    CallingNumber = "123456783",
                    BuyRate = 2M,
                    Duration = 10,
                    ProviderId = 2,
                    DialCode = "1234",
                },
            };

            using var data = DatabaseMock.Instance;

            var billingService = new BillingService(configuration, data);

            data.Records.AddRange(records);
            data.SaveChanges();

            // Act
            var result = billingService.CostByProviderId(providerId);

            // Assert
            Assert.Equal((buyRate * duration) / 60, result);
        }

        [Fact]
        public void DoesReturnCostByGivenDateInterval()
        {
            // Arrange
            var startDate = new DateTime(2022, 5,1);
            var endDate = new DateTime(2022, 5, 31);
            var buyRate = 1M;
            var duration = 60;
            var myConfiguration = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Nested:Key1", "NestedValue1" },
                { "Nested:Key2", "NestedValue2" },
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            var records = new List<UserEx.Data.Models.Record>()
            {
                new UserEx.Data.Models.Record()
                {
                    Date = new DateTime(2022, 5, 10),
                    CallerNumberNotProcured = "123456789",
                    CallingNumber = "123456780",
                    BuyRate = buyRate,
                    Duration = duration,
                    ProviderId = 1,
                    DialCode = "123",
                },
                new UserEx.Data.Models.Record()
                {
                    Date = new DateTime(2022, 6, 10),
                    CallerNumberNotProcured = "123456782",
                    CallingNumber = "123456783",
                    BuyRate = 2M,
                    Duration = 10,
                    ProviderId = 2,
                    DialCode = "1234",
                },
            };

            using var data = DatabaseMock.Instance;

            var billingService = new BillingService(configuration, data);

            data.Records.AddRange(records);
            data.SaveChanges();

            // Act
            var result = billingService.CostAllProviderByDate(startDate, endDate);

            // Assert
            Assert.Equal((buyRate * duration)/60, result);
        }

        [Fact]
        public void DoesReturnCostForTheProcuredNumbers()
        {
            // Arrange
            var startDate = new DateTime(2022, 5, 1);
            var endDate = new DateTime(2022, 5, 31);
            var buyRate = 1M;
            var duration = 60;
            var monthlyPrice = 30M;
            var myConfiguration = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Nested:Key1", "NestedValue1" },
                { "Nested:Key2", "NestedValue2" },
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            var records = new List<UserEx.Data.Models.Record>()
            {
                new UserEx.Data.Models.Record()
                {
                    Date = new DateTime(2022, 5, 10),
                    CallerNumberNotProcured = "123456789",
                    CallingNumber = "123456780",
                    NumberId = null,
                    BuyRate = buyRate,
                    Duration = duration,
                    ProviderId = 1,
                    DialCode = "123",
                },
                new UserEx.Data.Models.Record()
                {
                    Date = new DateTime(2022, 6, 10),
                    CallerNumberNotProcured = null,
                    CallingNumber = "123456783",
                    BuyRate = 2M,
                    Duration = 10,
                    ProviderId = 2,
                    DialCode = "1234",
                    NumberId = 3,
                    Number = new Number
                    {
                        Id = 3,
                        PartnerId = 1,
                        DidNumber = "0987654323",
                        Description = "TestDescription3",
                        StartDate = DateTime.Now,
                        ProviderId = 1,
                        Source = 0,
                        IsActive = true,
                        MonthlyPrice = monthlyPrice,
                        SetupPrice = 30M,
                        IsPublic = true,
                    },
                },
            };

            using var data = DatabaseMock.Instance;

            var billingService = new BillingService(configuration, data);

            data.Records.AddRange(records);
            data.SaveChanges();

            // Act
            var result = billingService.CostProcuredNumbers();

            // Assert
            Assert.Equal(monthlyPrice, result);
        }

        [Fact]
        public void DoesReturnTrueIfProviderNameExists()
        {
            // Arrange
            var providerName = "test";
            var providerId = 1;
            var myConfiguration = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Nested:Key1", "NestedValue1" },
                { "Nested:Key2", "NestedValue2" },
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

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

            var providers = new BillingService(configuration, data);

            data.Providers.AddRange(providersList);
            data.SaveChanges();

            // Act
            var result = providers.ProviderExists(providerId);

            // Assert
            Assert.True(result);
        }
    }
}
