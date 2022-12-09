namespace UserEx.Services.Data.Numbers.Models
{
    using System.Collections.Generic;

    using UserEx.Web.ViewModels.Numbers;

    public class NumberQueryServiceModel
    {
        public int NumbersPerPage { get; set; }

        public string Provider { get; init; }

        public string SearchTerm { get; init; }

        public NumberSorting Sorting { get; init; }

        public int CurrentPage { get; init; }

        public int TotalNumbers { get; set; }

        public IEnumerable<NumberServiceModel> Numbers { get; set; }
    }
}
