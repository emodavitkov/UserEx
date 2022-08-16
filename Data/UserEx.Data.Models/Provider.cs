namespace UserEx.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Provider
    {
        public int Id { get; init; }

        public string Name { get; set; }

        public IEnumerable<Number> Numbers { get; init; } = new List<Number>();
    }
}
