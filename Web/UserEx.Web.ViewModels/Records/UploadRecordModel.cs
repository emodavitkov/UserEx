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

    public class UploadRecordModel
    {
        // to add validation rules
        public DateTime Date { get; set; }

        [Required]
        public string CallerNumber { get; set; }

        [Required]
        public string CallingNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal BuyRate { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int ProviderId { get; init; }

        [Required]
        public string DialCode { get; set; }
    }
}
