namespace UserEx.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Number
    {
        public int Id { get; init; }

        [Required]
        public string DidNumber { get; set; }

        public string OrderReference { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal SetupPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MonthlyPrice { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime? EndDate { get; set; }

        // public string Source { get; set; } = "manual"
        public SourceEnum Source { get; set; }

        public bool IsPublic { get; set; }

        public int ProviderId { get; set; }

        public Provider Provider { get; init; }

        public int? PartnerId { get; init; }

        public Partner Partner { get; init; }
    }
}
