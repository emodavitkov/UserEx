namespace UserEx.Services.Data.Partners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data;

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
           => this.data
            .Partners
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstOrDefault();
    }
}
