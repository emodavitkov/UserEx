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
    }
}
