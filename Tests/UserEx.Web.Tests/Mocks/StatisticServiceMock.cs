namespace UserEx.Web.Tests.Mocks
{
    using Moq;
    using UserEx.Services.Data.Statistics;

    public static class StatisticServiceMock
    {
        public static IStatisticsService Instance
        {
            get
            {
                var statisticsServiceMock = new Mock<IStatisticsService>();

                statisticsServiceMock
                    .Setup(s => s.Total())
                    .Returns(new StatisticsServiceModel
                    {
                        TotalUsers = 10,
                        TotalNumbersApproved = 100,
                        TotalNumbersNotApproved = 10,
                    });
                return statisticsServiceMock.Object;
            }
        }
    }
}
