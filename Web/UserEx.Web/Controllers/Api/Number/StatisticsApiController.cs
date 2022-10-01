namespace UserEx.Web.Controllers.Api.Number
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : Controller
    {
        private readonly ApplicationDbContext data;

        public StatisticsApiController(ApplicationDbContext data)
            => this.data = data;

        [HttpGet]
        public StatisticsResponseModel GetStatistics()
        {
            var totalNumbers = this.data.Numbers.Count();
            var totalUsers = this.data.Users.Count();

            var statistics = new StatisticsResponseModel
            {
                TotalNumbers = totalNumbers,
                TotalUsers = totalUsers,
                TotalMonthlyCost = 0,
            };
            return statistics;
        }
    }
}
