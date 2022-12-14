namespace UserEx.Services.Data.Numbers.Models
{
    using System.Collections.Generic;

    public class AllNumberServiceAdminModel
    {
        public const int NumbersPerPage = 20;

        public int CurrentPage { get; init; } = 1;

        public int TotalNumbers { get; set; }

        public string Filter { get; init; }

        public bool IsPublic { get; init; }

        public IEnumerable<NumberServiceModel> Numbers { get; set; }
    }
}
