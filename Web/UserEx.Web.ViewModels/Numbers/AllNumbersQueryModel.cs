namespace UserEx.Web.ViewModels.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllNumbersQueryModel
    {
        // brands
        public string Provider { get; init; }

        public IEnumerable<string> Providers { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public NumberSorting Sorting { get; init; }

        public IEnumerable<NumberListingViewModel> Numbers { get; init; }
    }
}
