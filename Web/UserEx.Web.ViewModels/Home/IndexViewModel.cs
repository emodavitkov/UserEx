namespace UserEx.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int TotalNumbers { get; init; }

        public int TotalUsers { get; set; }

        // public int TotalMonthlyCost { get; set; }

        public List<NumberIndexViewModel> Numbers { get; init; }
    }
}
