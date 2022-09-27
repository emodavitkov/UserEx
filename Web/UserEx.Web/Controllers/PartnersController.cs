namespace UserEx.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using UserEx.Data;
    using UserEx.Data.Common;
    using UserEx.Data.Models;
    using UserEx.Web.ViewModels.Partners;

    public class PartnersController : Controller
    {
        private readonly ApplicationDbContext data;

        public PartnersController(ApplicationDbContext data)
        {
            this.data = data;
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
            var userIdAlreadyPartner = this.data
                .Partners
                .Any(p => p.UserId == userId);

            if (userIdAlreadyPartner)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(partner);
            }

            var partnerData = new Partner
            {
                OfficeName = partner.OfficeName,
                PhoneNumber = partner.PhoneNumber,
                UserId = userId,
            };
            this.data.Partners.Add(partnerData);
            this.data.SaveChanges();

            return this.RedirectToAction("All", "Numbers");
        }
    }
}
