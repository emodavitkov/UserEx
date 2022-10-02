namespace UserEx.Services.Data.Numbers.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Web.ViewModels.Numbers;

    public class NumberQueryServiceModel
    {
        // public const int NumbersPerPage = 20;

        public int NumbersPerPage { get; set; }

        public string Provider { get; init; }

        // public IEnumerable<string> Providers { get; set; }

        public string SearchTerm { get; init; }

        public NumberSorting Sorting { get; init; }

        // public int CurrentPage { get; init; } = 1;

        public int CurrentPage { get; init; }

        public int TotalNumbers { get; set; }

        public IEnumerable<NumberServiceModel> Numbers { get; set; }
    }
}
