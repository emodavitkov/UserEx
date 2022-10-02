namespace UserEx.Services.Data.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        IEnumerable<NumberServiceModel> ByUser(string userId);

        IEnumerable<string> AllNumberProviders();
    }
}
