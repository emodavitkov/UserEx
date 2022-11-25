namespace UserEx.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data.Common;
    using UserEx.Services.Data.Billing;
    using UserEx.Services.Data.Numbers;
    using UserEx.Web.ViewModels.Billing;

    public class BillingController : AdministrationController
    {
        // private readonly ApplicationDbContext data;
        private readonly IBillingService billing;
        private readonly INumberService number;

        public BillingController(
            IBillingService billing,
            INumberService number)
        {
            this.billing = billing;
            this.number = number;
        }

        // public IActionResult Index()
        // {
        //    var totalNumbersApproved = this.data.Numbers.Count(n => n.IsPublic);
        //    var totalNumbersNotApproved = this.data.Numbers.Count(n => n.IsPublic == false);
        //    var totalUsers = this.data.Users.Count();
        //    return this.Ok();
        // }
        [Authorize]
        public IActionResult Billing()
        {
            var userId = this.User.GetId();

            if (!this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            return this.View(new BillingResponseModel()
            {
                Providers = this.billing.AllNumberProviders(),
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Billing(BillingResponseModel billingModel)
        {
            if (!this.billing.ProviderExists(billingModel.ProviderId))
            {
                this.ModelState.AddModelError(nameof(billingModel.ProviderId), "Provider does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                // number.Providers = this.GetNumberProviders();
                billingModel.Providers = this.number.AllNumberProviders();

                return this.View(billingModel);
            }

            var resultCostByProviderId = this.billing.CostByProviderId(billingModel.ProviderId);
            var resultCostByDate = this.billing.CostAllProviderByDate(billingModel.StartDate, billingModel.EndDate);
            var resultProcuredNumbers = this.billing.CostProcuredNumbers();

            var billingResultData = new BillingResultDataModel
            {
                SumCostByProvider = resultCostByProviderId,
                SumCostByDate = resultCostByDate,
                SumProcuredNumbers = resultProcuredNumbers,
            };

            return this.View("BillingResultData", billingResultData);
        }

        [Authorize]
        public async Task<IActionResult> Balance()
        {
            var balance = await this.billing.GetBalance();
            return this.View(balance);
        }

        [Authorize]
        public IActionResult BillingCostByProvider()
        {
            var costByProvider = this.billing.CostByProviderId(3);

            var result = new BillingCostByProviderModel
            {
                SumCostByProvider = costByProvider,
            };
            return this.View(result);
        }
    }
}
