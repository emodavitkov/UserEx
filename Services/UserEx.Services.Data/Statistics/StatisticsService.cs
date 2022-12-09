namespace UserEx.Services.Data.Statistics
{
    using System.Linq;

    using UserEx.Data;

    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext data;

        public StatisticsService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public StatisticsServiceModel Total()
        {
            var totalNumbersApproved = this.data.Numbers.Count(n => n.IsPublic);
            var totalNumbersNotApproved = this.data.Numbers.Count(n => n.IsPublic == false);
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalNumbersApproved = totalNumbersApproved,
                TotalUsers = totalUsers,
                TotalNumbersNotApproved = totalNumbersNotApproved,
            };
        }
    }
}
