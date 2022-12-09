namespace UserEx.Web.Areas.Administration.Controllers.Api.Numbers
{

    using Microsoft.AspNetCore.Mvc;
    using UserEx.Services.Data.Statistics;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : AdministrationController
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
