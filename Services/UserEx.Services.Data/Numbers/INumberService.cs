namespace UserEx.Services.Data.Numbers
{
    using System;
    using System.Collections.Generic;

    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers.Models;
    using UserEx.Web.ViewModels.Home;
    using UserEx.Web.ViewModels.Numbers;

    public interface INumberService
    {
        NumberQueryServiceModel All(
            string provider = null,
            string searchTerm = null,
            NumberSorting sorting = NumberSorting.DateCreated,
            int currentPage = 1,
            int numbersPerPage = int.MaxValue,
            bool? publicOnly = null);

        IEnumerable<NumberIndexViewModel> Latest();

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

        int BulkCreate(List<NumberManualModel> bulkNumbers, int partnerId);

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
            DateTime? endDate,
            bool isPublic);

        int Delete(
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
            DateTime? endDate,
            bool isPublic);

        IEnumerable<NumberServiceModel> ByUser(string userId);

        bool NumberIsByPartner(int numberId, int partnerId);

        void ChangeVisibility(int numberId);

        IEnumerable<string> AllNumbersByProvider();

        IEnumerable<NumberProviderViewModel> AllNumberProviders();

        bool ProviderExists(int providerId);
    }
}
