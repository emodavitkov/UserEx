namespace UserEx.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
