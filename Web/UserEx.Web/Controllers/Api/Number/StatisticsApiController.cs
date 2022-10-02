namespace UserEx.Web.Controllers.Api.Number
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data;
    using UserEx.Services.Data.Statistics;
    using UserEx.Web.ViewModels.Api;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : Controller
    {
        private readonly IStatisticsService statistics;

        public StatisticsApiController(IStatisticsService statistics)
            => this.statistics = statistics;

        [HttpGet]
        public StatisticsServiceModel GetStatistics()
        {
            return this.statistics.Total();
        }
    }
}
