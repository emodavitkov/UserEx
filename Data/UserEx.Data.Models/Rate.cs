namespace UserEx.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Rate
    {
        [Required]
        public string DialCode { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public string DestinationName { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Cost { get; set; }

        public Provider Provider { get; init; }
    }
}
