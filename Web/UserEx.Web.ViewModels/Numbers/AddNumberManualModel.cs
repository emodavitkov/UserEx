namespace UserEx.Web.ViewModels.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AddNumberManualModel
    {
        public const int NumberMinLength = 5;
        public const int NumberMaxLength = 15;
        public const int NumberDescriptionMinLength = 2;

        [Required]
        [StringLength(NumberMaxLength, MinimumLength = NumberMinLength)]
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

        [Required]
        [StringLength(
            int.MaxValue,
            MinimumLength = NumberDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
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
