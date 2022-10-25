namespace UserEx.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Record
    {
        public int Id { get; init; }

        public DateTime Date { get; set; }

        [Required]
        public string CallerNumber { get; set; }

        [Required]
        public string CallingNumber { get; set; }

        // public int NumberId { get; set; }
        // public Number Number { get; set; }
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal BuyRate { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int ProviderId { get; init; }

        [Required]
        public string DialCode { get; set; }

        [ForeignKey("ProviderId, DialCode")]
        public Rate Rate { get; set; }
    }
}
