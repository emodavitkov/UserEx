namespace UserEx.Services.Data.Partners
{
    using System;
    using System.Linq;

    using UserEx.Data;
    using UserEx.Data.Models;
    using UserEx.Web.ViewModels.Partners;

    public class PartnerService : IPartnerService
    {
        private readonly ApplicationDbContext data;

        public PartnerService(ApplicationDbContext data)
            => this.data = data;

        public bool IsPartner(string userId)
            => this.data
                .Partners
                .Any(p => p.UserId == userId);

        public int GetIdByUser(string userId)
        {
            var result = this.data
                .Partners
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            if (result == null)
            {
                throw new ArgumentNullException();
            }

            return result.Id;
        }

        public void SetUp(SetUpPartnerFormModel partner, string userId)
        {
            var partnerData = new Partner
            {
                OfficeName = partner.OfficeName,
                PhoneNumber = partner.PhoneNumber,
                UserId = userId,
            };
            this.data.Partners.Add(partnerData);
            this.data.SaveChanges();
        }
    }
}
