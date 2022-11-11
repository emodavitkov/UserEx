namespace UserEx.Services.Data.Partners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Web.ViewModels.Partners;

    public interface IPartnerService
    {
        public bool IsPartner(string userId);

        public int GetIdByUser(string userId);

        public void SetUp(SetUpPartnerFormModel partner, string userId);
    }
}
