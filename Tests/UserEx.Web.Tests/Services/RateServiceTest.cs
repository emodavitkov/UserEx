namespace UserEx.Web.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Partners;
    using UserEx.Services.Data.Rates;
    using UserEx.Web.Tests.Mocks;
    using UserEx.Web.ViewModels.Numbers;
    using UserEx.Web.ViewModels.Partners;
    using UserEx.Web.ViewModels.Rates;
    using Xunit;

    public class RateServiceTest
    {
        [Fact]
        public void DoesRateUploaded()
        {
            // Arrange
            var providerId = 1;

            var rateUpload = new List<UploadRateModel>()
            {
                new UploadRateModel()
                {
                    DestinationName = "Bulgaria",
                    DialCode = "359",
                    Cost = 2M,
                    ProviderId = 1,
                },
                new UploadRateModel()
                {
                    DestinationName = "Germany",
                    DialCode = "49",
                    Cost = 20M,
                    ProviderId = 2,
                },
            };

            using var data = DatabaseMock.Instance;

            var rateService = new RateService(data);

            // data.Rates.AddRange(rateUpload);
           // data.SaveChanges();

            // Act
            rateService.Upload(providerId, rateUpload);

            // Assert
            Assert.Equal(2, data.Rates.Count());
        }
    }
}
