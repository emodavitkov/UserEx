namespace UserEx.Web.ViewModels.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data.Models;

    public class AddNumberManualModel
    {
        public const int NumberMinLength = 5;
        public const int NumberMaxLength = 15;

        [Required]
        // [Range(NumberMinLength, NumberMaxLength)]
        public string DidNumber { get; set; }

        public string OrderReference { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Setup Price")]
        public decimal SetupPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Monthly Price")]
        public decimal MonthlyPrice { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime? EndDate { get; set; }

        [Display(Name="Provider")]
        public int ProviderId { get; set; }

        public IEnumerable<NumberProviderViewModel> Providers { get; set; }
    }
}
