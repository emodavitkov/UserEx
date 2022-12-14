namespace UserEx.Services.Data.Numbers.Models
{
    using System;

    public class NumberDetailsServiceModel : NumberServiceModel
    {
        public string OrderReference { get; set; }

        public decimal SetupPrice { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }

        public int? PartnerId { get; set; }

        public int ProviderId { get; set; }

        public string PartnerName { get; set; }

        public string UserId { get; init; }
    }
}
