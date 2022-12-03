namespace UserEx.Services.Data.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PhoneNumbers;
    using UserEx.Data;
    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers.Models;
    using UserEx.Web.ViewModels.Home;
    using UserEx.Web.ViewModels.Numbers;

    public class NumberService : INumberService
    {
        private readonly ApplicationDbContext data;
        private static PhoneNumberUtil phoneUtil;

        public NumberService(ApplicationDbContext data)
        {
            this.data = data;
            phoneUtil = PhoneNumberUtil.GetInstance();
        }

        // bool publicOnly = true)
        public NumberQueryServiceModel All(
            string provider = null,
            string searchTerm = null,
            NumberSorting sorting = NumberSorting.DateCreated,
            int currentPage = 1,
            int numbersPerPage = int.MaxValue,
            bool? publicOnly = null)
        {
            // isPublic addition
            // var numbersQuery = this.data.Numbers.AsQueryable();
            // add filter to the Admin All numbers view
            // var numbersQuery = this.data.Numbers
            //        .Where(n => !publicOnly || n.IsPublic);
            var numbersQuery = this.data.Numbers.AsQueryable();
            numbersQuery = publicOnly.HasValue ? numbersQuery.Where(n => n.IsPublic == publicOnly) : numbersQuery;

            if (!string.IsNullOrWhiteSpace(provider))
            {
                numbersQuery = numbersQuery.Where(n => n.Provider.Name == provider);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                numbersQuery = numbersQuery.Where(n =>

                    // (n.DidNumber + " " + n.Provider.Name).ToLower().Contains(searchTerm.ToLower()) ||
                    n.DidNumber.ToLower().Contains(searchTerm.ToLower()) ||
                    n.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            numbersQuery = sorting switch
            {
                NumberSorting.DateCreated => numbersQuery.OrderByDescending(n => n.StartDate),
                NumberSorting.MonthlyPrice => numbersQuery.OrderByDescending(n => n.MonthlyPrice),
                NumberSorting.Description => numbersQuery.OrderBy(n => n.Description),
                _ => numbersQuery.OrderByDescending(n => n.Id),

                // NumberSorting.Description or _ => numbersQuery.OrderByDescending(n => n.Id),
                // _ => carsQuery.OrderByDescending(c => c.Id)
            };

            // var totalNumbers = this.data.Numbers.Count();
            var totalNumbers = numbersQuery.Count();

            var numbers = GetNumbers(numbersQuery
                .Skip((currentPage - 1) * numbersPerPage)
                .Take(numbersPerPage));

            // getNumbers above
            // var numbers = numbersQuery
            //    .Skip((currentPage - 1) * numbersPerPage)
            //    .Take(numbersPerPage)
            //    // .OrderByDescending(n => n.Id)
            //    .Select(n => new NumberServiceModel()
            //    {
            //        Id = n.Id,
            //        DidNumber = n.DidNumber,
            //        MonthlyPrice = n.MonthlyPrice,
            //        Description = n.Description,
            //        Provider = n.Provider.Name,
            //    })
            //    .ToList();

            //// brand
            // var numberProviders = this.data
            //    .Numbers
            //    .Select(p => p.Provider.Name)
            //    .Distinct()
            //    .OrderBy(p => p)
            //    .ToList();
            return new NumberQueryServiceModel
            {
                TotalNumbers = totalNumbers,
                CurrentPage = currentPage,
                NumbersPerPage = numbersPerPage,
                Numbers = numbers,
            };
        }

        // issue with LatestNumbersServiceModel
        public IEnumerable<NumberIndexViewModel> Latest()
            => this.data
                .Numbers
                .Where(n => n.IsPublic)
                .OrderByDescending(n => n.Id)
                .Select(n => new NumberIndexViewModel()
                {
                    Id = n.Id,
                    DidNumber = n.DidNumber,
                    MonthlyPrice = n.MonthlyPrice,
                    Description = n.Description,
                })
                .Take(3)
                .ToList();

        // public IEnumerable<LatestNumbersServiceModel> Latest()
        //    => this.data
        //        .Numbers
        //        .OrderByDescending(n => n.Id)
        //        .Select(n => new LatestNumbersServiceModel()
        //        {
        //            Id = n.Id,
        //            DidNumber = n.DidNumber,
        //            MonthlyPrice = n.MonthlyPrice,
        //            Description = n.Description,
        //        })
        //        .Take(3)
        //        .ToList();
        public NumberDetailsServiceModel Details(int id)
            => this.data
                .Numbers
                .Where(n => n.Id == id)
                .Select(n => new NumberDetailsServiceModel
                {
                    Id = n.Id,
                    UserId = n.Partner.UserId,
                    DidNumber = n.DidNumber,
                    OrderReference = n.OrderReference,
                    MonthlyPrice = n.MonthlyPrice,
                    SetupPrice = n.SetupPrice,
                    StartDate = n.StartDate,
                    IsActive = n.IsActive,
                    Description = n.Description,
                    Provider = n.Provider.Name,
                    ProviderId = n.ProviderId,
                    PartnerId = n.PartnerId.Value,
                    PartnerName = n.Partner.OfficeName,
                })
                .FirstOrDefault();

        public int Create(
            int providerId,
            string didNumber,
            string orderReference,
            decimal setupPrice,
            decimal monthlyPrice,
            string description,
            bool isActive,
            SourceEnum source,
            DateTime startDate,
            DateTime? endDate,
            int partnerId)
        {
            var numberData = new Number
            {
                ProviderId = providerId,
                DidNumber = didNumber,
                OrderReference = orderReference,
                SetupPrice = setupPrice,
                MonthlyPrice = monthlyPrice,
                Description = description,
                Source = SourceEnum.Manual,
                IsActive = isActive,
                StartDate = startDate,
                EndDate = endDate,
                PartnerId = partnerId,
                IsPublic = false,
            };

            if (this.data.Numbers.FirstOrDefault(n => n.DidNumber == numberData.DidNumber) == null)
            {
                this.data.Numbers.Add(numberData);
                return this.data.SaveChanges();
            }

            // return numberData.Id;
            return 0;
        }

        public int BulkCreate(List<NumberManualModel> bulkNumbers, int partnerId)
        {
            foreach (var number in bulkNumbers)
            {
                bool phoneNumberIsValid;

                try
                {
                    var phoneNumber = phoneUtil.Parse($"+{number.DidNumber}", null);

                    phoneNumberIsValid = phoneUtil.IsValidNumber(phoneNumber);
                }
                catch (Exception)
                {
                    continue;
                }

                if (this.data.Numbers.FirstOrDefault(n => n.DidNumber == number.DidNumber) != null || !phoneNumberIsValid)
                {
                    continue;
                }

                var numberFromExcel = new Number
                {
                    DidNumber = number.DidNumber,
                    Description = number.Description,
                    StartDate = number.StartDate,
                    EndDate = number.EndDate,
                    ProviderId = (int)number.ProviderId,
                    Source = number.Source,
                    IsActive = true,
                    IsPublic = false,
                    MonthlyPrice = number.MonthlyPrice,
                    SetupPrice = number.SetupPrice,
                    PartnerId = partnerId,
                };
                this.data.Numbers.Add(numberFromExcel);
            }

            return this.data.SaveChanges();
        }

        public bool Edit(
            int id,
            int providerId,
            string didNumber,
            string orderReference,
            decimal setupPrice,
            decimal monthlyPrice,
            string description,
            bool isActive,
            SourceEnum source,
            DateTime startDate,
            DateTime? endDate,
            bool isPublic)
        {
            var numberData = this.data.Numbers.Find(id);

            if (numberData == null)
            {
               return false;
            }

            numberData.ProviderId = providerId;
            numberData.DidNumber = didNumber;
            numberData.OrderReference = orderReference;
            numberData.SetupPrice = setupPrice;
            numberData.MonthlyPrice = monthlyPrice;
            numberData.Description = description;

            // numberData.Source = SourceEnum.Manual;
            numberData.IsActive = isActive;
            numberData.StartDate = startDate;
            numberData.EndDate = endDate;
            numberData.IsPublic = isPublic;

            // numberData.PartnerId = partnerId;
            this.data.SaveChanges();
            return true;
        }

        public int Delete(
            int numberId,
            int providerId,
            string didNumber,
            string orderReference,
            decimal setupPrice,
            decimal monthlyPrice,
            string description,
            bool isActive,
            SourceEnum source,
            DateTime startDate,
            DateTime? endDate,
            bool isPublic)
        {
            var numberData = this.data.Numbers.Find(numberId);

            numberData.ProviderId = providerId;
            numberData.DidNumber = didNumber;
            numberData.OrderReference = orderReference;
            numberData.SetupPrice = setupPrice;
            numberData.MonthlyPrice = monthlyPrice;
            numberData.Description = description;

            // numberData.Source = SourceEnum.Manual;
            numberData.IsActive = false;
            numberData.StartDate = startDate;
            numberData.EndDate = DateTime.Now;
            numberData.IsPublic = isPublic;

            // numberData.PartnerId = partnerId;
            this.data.Remove(numberData);
            this.data.SaveChanges();

            return numberData.Id;
        }

        public IEnumerable<NumberServiceModel> ByUser(string userId)
            => GetNumbers(this.data
                .Numbers
                .Where(n => n.Partner.UserId == userId));

        public void ChangeVisibility(int numberId)
        {
            var number = this.data.Numbers.Find(numberId);

            number.IsPublic = !number.IsPublic;

            this.data.SaveChanges();
        }

        public bool NumberIsByPartner(int numberId, int partnerId)
            => this.data
                .Numbers
                .Any(n => n.Id == numberId && n.PartnerId == partnerId);

        public IEnumerable<string> AllNumbersByProvider()
          => this.data
             .Numbers
             .Select(p => p.Provider.Name)
             .Distinct()
             .OrderBy(p => p)
             .ToList();

        public IEnumerable<NumberProviderViewModel> AllNumberProviders()
            => this.data
                .Providers
                .Select(p => new NumberProviderViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                })
                .ToList();

        public bool ProviderExists(int providerId)
            => this.data
                .Providers
                .Any(p => p.Id == providerId);

        // public IEnumerable<NumberProviderServiceModel> AllNumberProviders()
        //    => this.data
        //        .Providers
        //        .Select(p => new NumberProviderServiceModel()
        //        {
        //            Id = p.Id,
        //            Name = p.Name,
        //        })
        //        .ToList();
        private static IEnumerable<NumberServiceModel> GetNumbers(IQueryable<Number> numberQuery)
            => numberQuery
                .Select(n => new NumberServiceModel
                {
                    Id = n.Id,
                    DidNumber = n.DidNumber,
                    MonthlyPrice = n.MonthlyPrice,
                    Description = n.Description,
                    Provider = n.Provider.Name,
                    IsPublic = n.IsPublic,
                })
                .ToList();
    }
}
