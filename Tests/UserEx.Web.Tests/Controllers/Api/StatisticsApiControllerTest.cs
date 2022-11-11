namespace UserEx.Web.Tests.Controllers.Api
{
    using UserEx.Web.Areas.Administration.Controllers.Api.Numbers;
    using UserEx.Web.Tests.Mocks;
    using Xunit;

    public class StatisticsApiControllerTest
    {
        [Fact]
        public void GetStatisticsShouldReturnTotalStatistics()
        {
            // Arrange
            var statisticsController = new StatisticsApiController(StatisticServiceMock.Instance);

            // Act
            var result = statisticsController.GetStatistics();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.TotalUsers);
            Assert.Equal(100, result.TotalNumbersApproved);
            Assert.Equal(10, result.TotalNumbersNotApproved);
        }
    }
}
