namespace UserEx.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IndexViewModel
    {
        public int TotalNumbers { get; init; }

        public int TotalUsers { get; set; }

        public int TotalX { get; set; }

        public List<NumberIndexViewModel> Numbers { get; init; }
    }
}
