namespace UserEx.Services.Data.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers.Models;
    using UserEx.Web.ViewModels.Numbers;

    public interface INumberService
    {
        NumberQueryServiceModel All(
            string provider,
            string searchTerm,
            NumberSorting sorting,
            int currentPage,
            int numbersPerPage);

        NumberDetailsServiceModel Details(int numberId);

        int Create(
            int providerId,
            string didNumber,
            string orderReference,
            decimal setupPrice,
            decimal monthlyPrice,
            string description,
            bool isActive,
            SourceEnum source,
            DateTime startDate,
            DateTime? endDate,
            int partnerId);

        bool Edit(
            int numberId,
            int providerId,
            string didNumber,
            string orderReference,
            decimal setupPrice,
            decimal monthlyPrice,
            string description,
            bool isActive,
            SourceEnum source,
            DateTime startDate,
            DateTime? endDate);
        // int partnerId

        IEnumerable<NumberServiceModel> ByUser(string userId);

        bool NumberIsByPartner(int numberId, int partnerId);

        // IEnumerable<string> AllNumberProviders();
        IEnumerable<string> AllNumbersByProvider();

        // IEnumerable<NumberProviderServiceModel> AllNumberProviders();
        IEnumerable<NumberProviderViewModel> AllNumberProviders();

        bool ProviderExists(int providerId);
    }
}
