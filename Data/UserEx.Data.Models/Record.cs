namespace UserEx.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Record
    {
        public int Id { get; init; }

        public DateTime Date { get; set; }

        public string CallerNumberNotProcured { get; set; }

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

        [ForeignKey("ProviderId, DialCode")]

        public Rate Rate { get; set; }

        // link to numbers
        public int? NumberId { get; set; }

        public Number Number { get; set; }
    }
}
