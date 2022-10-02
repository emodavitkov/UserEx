namespace UserEx.Web.ViewModels.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Services;

    public class AllNumbersQueryModel
    {
        public const int NumbersPerPage = 20;

        // brands
        public string Provider { get; init; }

        public IEnumerable<string> Providers { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public NumberSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalNumbers { get; set; }

        // Cannot add this model?
        // public IEnumerable<NumberServiceModel> Numbers { get; set; }
        public IEnumerable<NumberListingViewModel> Numbers { get; set; }
    }
}
