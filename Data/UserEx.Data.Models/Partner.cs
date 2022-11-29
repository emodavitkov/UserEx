namespace UserEx.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static UserEx.Data.Common.DataConstants.Partner;

    public class Partner
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(OfficeNameMaxLength)]
        public string OfficeName { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        // public ApplicationUser User { get; set; }
        public IEnumerable<Number> Numbers { get; init; } = new List<Number>();
    }
}
