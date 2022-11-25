namespace UserEx.Web.ViewModels.Partners
{
    using System.ComponentModel.DataAnnotations;

    using static UserEx.Data.Common.DataConstants.Partner;

    public class SetUpPartnerFormModel
    {
        [Required]
        [StringLength(OfficeNameMaxLength, MinimumLength = OfficeNameMinLength)]
        [Display(Name = "Partner Office Name")]
        public string OfficeName { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
