namespace UserEx.Web.Areas.Administration.Controllers.Api.Numbers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data;
    using UserEx.Services.Data.Statistics;
    using UserEx.Web.ViewModels.Api;

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
