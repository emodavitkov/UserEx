namespace UserEx.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NumberIndexViewModel
    {
        public int Id { get; init; }

        public string DidNumber { get; init; }

        public decimal MonthlyPrice { get; init; }

        public string Description { get; init; }
    }
}
