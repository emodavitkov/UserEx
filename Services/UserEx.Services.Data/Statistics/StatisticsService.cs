namespace UserEx.Services.Data.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
