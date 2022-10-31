namespace UserEx.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data;
    using UserEx.Data.Common;
    using UserEx.Data.Models;
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

            // move to service
            var userIdAlreadyPartner = this.partner.IsPartner(userId);

            // var userIdAlreadyPartner = this.data
            //    .Partners
            //    .Any(p => p.UserId == userId);
            if (userIdAlreadyPartner)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(partner);
            }

            this.partner.SetUp(partner, userId);

            // var partnerData = new Partner
            // {
            //    OfficeName = partner.OfficeName,
            //    PhoneNumber = partner.PhoneNumber,
            //    UserId = userId,
            // };
            // this.data.Partners.Add(partnerData);
            // this.data.SaveChanges();
            this.TempData[GlobalMessageKey] = "Thank you for being a Partner!";

            return this.RedirectToAction("All", "Numbers");
        }
    }
}
