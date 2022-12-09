namespace UserEx.Web.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using UserEx.Services.Data.Rates;
    using UserEx.Web.Tests.Mocks;
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

            // Act
            rateService.Upload(providerId, rateUpload);

            // Assert
            Assert.Equal(2, data.Rates.Count());
        }
    }
}
