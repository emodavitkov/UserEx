namespace UserEx.Web.ViewModels.Rates
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using UserEx.Data.Models;
    using UserEx.Web.ViewModels.Numbers;

    using static UserEx.Data.Common.DataConstants.Rate;

    public class UploadRateModel
    {
        [Required]
        [StringLength(DialCodeNumberMaxLength, MinimumLength = DialCodeNumberMinLength)]
        public string DialCode { get; set; }

        [Required]
        public int ProviderId { get; set; }

        public Provider Provider { get; init; }

        [Required]
        [StringLength(DestinationNameMaxLength, MinimumLength = DestinationNameMinLength)]
        public string DestinationName { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Cost { get; set; }

        public IEnumerable<NumberProviderViewModel> Providers { get; set; }
    }
}
