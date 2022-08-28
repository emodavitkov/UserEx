namespace UserEx.Web.ViewModels.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NumberListingViewModel
    {
        public int Id { get; init; }

        public string DidNumber { get; init; }

        public decimal MonthlyPrice { get; init; }

        public string Description { get; init; }

        public string Provider { get; init; }
    }
}
