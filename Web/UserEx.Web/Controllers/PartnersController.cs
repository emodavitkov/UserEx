namespace UserEx.Web.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data.Common;
    using UserEx.Services.Data.Partners;
    using UserEx.Web.ViewModels.Partners;

    using static UserEx.Common.GlobalConstants;

    public class PartnersController : Controller
    {
        private readonly IPartnerService partner;

        public PartnersController(IPartnerService partner)
        {
            this.partner = partner;
        }

        [Authorize]
        public IActionResult SetUp()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult SetUp(SetUpPartnerFormModel partner)
        {
            var userId = this.User.GetId();

            var userIdAlreadyPartner = this.partner.IsPartner(userId);

            if (userIdAlreadyPartner)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(partner);
            }

            this.partner.SetUp(partner, userId);

            this.TempData[GlobalMessageKey] = "Thank you for being a Partner!";

            return this.RedirectToAction("All", "Numbers");
        }
    }
}
