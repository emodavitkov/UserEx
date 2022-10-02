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
            var totalNumbers = this.data.Numbers.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalNumbers = totalNumbers,
                TotalUsers = totalUsers,
                TotalMonthlyCost = 0,
            };
        }
    }
}
