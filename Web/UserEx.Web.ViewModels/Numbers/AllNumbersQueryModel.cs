namespace UserEx.Web.ViewModels.Numbers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllNumbersQueryModel
    {
        public const int NumbersPerPage = 20;

        public string Provider { get; init; }

        public IEnumerable<string> Providers { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public NumberSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalNumbers { get; set; }

        public IEnumerable<NumberListingViewModel> Numbers { get; set; }
    }
}
