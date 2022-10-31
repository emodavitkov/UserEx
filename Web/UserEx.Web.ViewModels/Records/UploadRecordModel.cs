namespace UserEx.Web.ViewModels.Records
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data.Models;
    using static UserEx.Data.Common.DataConstants.Records;

    public class UploadRecordModel
    {
        public DateTime Date { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string CallerNumber { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string CallingNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal BuyRate { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int ProviderId { get; init; }

        [Required]
        [StringLength(DialCodeNumberMaxLength, MinimumLength = DialCodeNumberMinLength)]
        public string DialCode { get; set; }
    }
}
