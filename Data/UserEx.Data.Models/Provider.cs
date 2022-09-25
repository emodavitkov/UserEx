namespace UserEx.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static UserEx.Data.Common.DataConstants.Provider;

    public class Provider
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public IEnumerable<Number> Numbers { get; init; } = new List<Number>();
    }
}
