namespace UserEx.Web.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Partners;
    using UserEx.Web.Tests.Mocks;
    using UserEx.Web.ViewModels.Numbers;
    using UserEx.Web.ViewModels.Partners;
    using Xunit;

    public class NumberServiceTest
    {
        [Fact]
        public void DoesBulkCreateReturnsTwoNumbersWhenListWithTwoIsGiven()
        {
            // Arrange
            var partnerId = 1;

            var numberList = new List<NumberManualModel>()
            {
                new NumberManualModel()
                {
                    DidNumber = "1234567890",
                    Description = "TestDescription",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 10M,
                    SetupPrice = 10M,
                },
                new NumberManualModel()
                {
                    DidNumber = "0987654321",
                    Description = "TestDescription1",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 20M,
                    SetupPrice = 20M,
                },
            };

            using var data = DatabaseMock.Instance;

            var numberService = new NumberService(data);

            // Act
            var result = numberService.BulkCreate(numberList, partnerId);

            // Assert
            Assert.Equal(numberList.Count, result);
        }

        [Fact]
        public void DoesAllReturnsTwoNumbersWhenListWithTwoIsGiven()
        {
            // Arrange
            var numberList = new List<Number>()
            {
                new Number()
                {
                    DidNumber = "1234567890",
                    Description = "TestDescription",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 10M,
                    SetupPrice = 10M,
                    IsPublic = true,
                },
                new Number()
                {
                    DidNumber = "0987654321",
                    Description = "TestDescription1",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 20M,
                    SetupPrice = 20M,
                    IsPublic = true,
                },
            };

            using var data = DatabaseMock.Instance;

            var numberService = new NumberService(data);

            data.Numbers.AddRange(numberList);
            data.SaveChanges();

            // Act
            var result = numberService.All();

            // Assert
            Assert.Equal(numberList.Count, result.TotalNumbers);
        }

        [Fact]
        public void DoesReturnsLastThreeNumbersWhenListWithFourIsGiven()
        {
            // Arrange
            var numberList = new List<Number>()
            {
                new Number()
                {
                    Id = 1,
                    DidNumber = "1234567890",
                    Description = "TestDescription",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 10M,
                    SetupPrice = 10M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 2,
                    DidNumber = "0987654321",
                    Description = "TestDescription1",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 20M,
                    SetupPrice = 20M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 3,
                    DidNumber = "0987654322",
                    Description = "TestDescription2",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 30M,
                    SetupPrice = 30M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 4,
                    DidNumber = "0987654323",
                    Description = "TestDescription3",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 50M,
                    SetupPrice = 50M,
                    IsPublic = true,
                },
            };

            using var data = DatabaseMock.Instance;

            var numberService = new NumberService(data);

            data.Numbers.AddRange(numberList);
            data.SaveChanges();

            // Act
            var result = numberService.Latest();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void DoesReturnsNumberDetailsByNumberId()
        {
            // Arrange
            var firstNumber = new Number()
            {
                Id = 1,
                Partner = new Partner
                {
                    UserId = "newTestUserId",
                    OfficeName = "TestPartnerOfficeName",
                    PhoneNumber = "1234567891",
                },
                Provider = new Provider
                {
                    Name = "TestName",
                },
                PartnerId = 1,
                DidNumber = "0987654320",
                Description = "TestDescription",
                StartDate = DateTime.Now,
                ProviderId = 1,
                Source = 0,
                IsActive = true,
                MonthlyPrice = 10M,
                SetupPrice = 10M,
                IsPublic = true,
            };

            var numberList = new List<Number>()
            {
                firstNumber,

                new Number()
                {
                    Id = 2,
                    Partner = new Partner
                    {
                        UserId = "newTestUserId2",
                        OfficeName = "TestPartnerOfficeName2",
                        PhoneNumber = "1234567892",
                    },
                    Provider = new Provider
                    {
                        Name = "TestName2",
                    },
                    PartnerId = 1,
                    DidNumber = "0987654322",
                    Description = "TestDescription2",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 20M,
                    SetupPrice = 20M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 3,
                    Partner = new Partner
                    {
                        UserId = "newTestUserId3",
                        OfficeName = "TestPartnerOfficeName3",
                        PhoneNumber = "1234567892",
                    },
                    Provider = new Provider
                    {
                        Name = "TestName3",
                    },
                    PartnerId = 1,
                    DidNumber = "0987654323",
                    Description = "TestDescription3",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 30M,
                    SetupPrice = 30M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 4,
                    Partner = new Partner
                    {
                        UserId = "newTestUserId3",
                        OfficeName = "TestPartnerOfficeName3",
                        PhoneNumber = "1234567893",
                    },
                    Provider = new Provider
                    {
                        Name = "TestName3",
                    },
                    PartnerId = 1,
                    DidNumber = "0987654322",
                    Description = "TestDescription3",
                    StartDate = DateTime.Now,
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 40M,
                    SetupPrice = 40M,
                    IsPublic = true,
                },
            };

            using var data = DatabaseMock.Instance;

            var numberService = new NumberService(data);

            data.Numbers.AddRange(numberList);
            data.SaveChanges();

            // Act
            var result = numberService.Details(1);

            // Assert
            Assert.Equal(firstNumber.ProviderId, result.ProviderId);
            Assert.Equal(firstNumber.DidNumber, result.DidNumber);
        }

        [Fact]
        public void DoesCreateNewNumber()
        {
            // Arrange
            var firstNumber = new Number()
            {
                Id = 1,
                PartnerId = 1,
                DidNumber = "0987654320",
                Description = "TestDescription",
                StartDate = DateTime.Now,
                OrderReference = "test",
                ProviderId = 1,
                Source = 0,
                IsActive = true,
                MonthlyPrice = 10M,
                SetupPrice = 10M,
                IsPublic = true,
            };

            var numberList = new List<Number>()
            {
                firstNumber,

                new Number()
                {
                    Id = 2,
                    PartnerId = 1,
                    DidNumber = "0987654321",
                    Description = "TestDescription1",
                    StartDate = DateTime.Now,
                    OrderReference = "test1",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 20M,
                    SetupPrice = 20M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 3,
                    PartnerId = 1,
                    DidNumber = "0987654322",
                    Description = "TestDescription2",
                    StartDate = DateTime.Now,
                    OrderReference = "test2",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 30M,
                    SetupPrice = 30M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 4,
                    PartnerId = 1,
                    DidNumber = "0987654323",
                    Description = "TestDescription3",
                    StartDate = DateTime.Now,
                    OrderReference = "test3",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 40M,
                    SetupPrice = 40M,
                    IsPublic = true,
                },
            };

            using var data = DatabaseMock.Instance;

            var numberService = new NumberService(data);

            data.Numbers.AddRange(numberList);
            data.SaveChanges();

            // Act
            var result = numberService.Create(
                1,
                "0987654325",
                "test",
                50M,
                50M,
                "TestDescription5",
                true,
                SourceEnum.Manual,
                DateTime.Now,
                null,
                1);

            // Assert
            Assert.Equal(numberList.Count + 1, data.Numbers.Count());
        }

        [Fact]
        public void DoesEditExistingNumber()
        {
            // Arrange
            var firstNumber = new Number()
            {
                Id = 1,
                PartnerId = 1,
                DidNumber = "0987654320",
                Description = "TestDescription",
                StartDate = DateTime.Now,
                OrderReference = "test",
                ProviderId = 1,
                Source = 0,
                IsActive = true,
                MonthlyPrice = 10M,
                SetupPrice = 10M,
                IsPublic = true,
            };

            var numberList = new List<Number>()
            {
                firstNumber,

                new Number()
                {
                    Id = 2,
                    PartnerId = 1,
                    DidNumber = "0987654321",
                    Description = "TestDescription1",
                    StartDate = DateTime.Now,
                    OrderReference = "test1",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 20M,
                    SetupPrice = 20M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 3,
                    PartnerId = 1,
                    DidNumber = "0987654322",
                    Description = "TestDescription2",
                    StartDate = DateTime.Now,
                    OrderReference = "test2",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 30M,
                    SetupPrice = 30M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 4,
                    PartnerId = 1,
                    DidNumber = "0987654323",
                    Description = "TestDescription3",
                    StartDate = DateTime.Now,
                    OrderReference = "test3",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 40M,
                    SetupPrice = 40M,
                    IsPublic = true,
                },
            };

            using var data = DatabaseMock.Instance;

            var numberService = new NumberService(data);

            data.Numbers.AddRange(numberList);
            data.SaveChanges();

            // Act
            var result = numberService.Edit(
                1,
                1,
                "0987654325",
                "test",
                50M,
                50M,
                "TestDescription5",
                true,
                SourceEnum.Manual,
                DateTime.Now,
                null,
                true);

            // Assert
            Assert.Equal("0987654325", data.Numbers.Where(x => x.Id == 1).Select(x => x.DidNumber).FirstOrDefault());
            Assert.Equal("TestDescription5", data.Numbers.Where(x => x.Id == 1).Select(x => x.Description).FirstOrDefault());
        }

        [Fact]
        public void DoesNumberDeleted()
        {
            // Arrange
            var firstNumber = new Number()
            {
                Id = 1,
                PartnerId = 1,
                DidNumber = "0987654320",
                Description = "TestDescription",
                StartDate = DateTime.Now,
                OrderReference = "test",
                ProviderId = 1,
                Source = 0,
                IsActive = true,
                MonthlyPrice = 10M,
                SetupPrice = 10M,
                IsPublic = true,
            };

            var numberList = new List<Number>()
            {
                firstNumber,

                new Number()
                {
                    Id = 2,
                    PartnerId = 1,
                    DidNumber = "0987654321",
                    Description = "TestDescription1",
                    StartDate = DateTime.Now,
                    OrderReference = "test1",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 20M,
                    SetupPrice = 20M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 3,
                    PartnerId = 1,
                    DidNumber = "0987654322",
                    Description = "TestDescription2",
                    StartDate = DateTime.Now,
                    OrderReference = "test2",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 30M,
                    SetupPrice = 30M,
                    IsPublic = true,
                },
                new Number()
                {
                    Id = 4,
                    PartnerId = 1,
                    DidNumber = "0987654323",
                    Description = "TestDescription3",
                    StartDate = DateTime.Now,
                    OrderReference = "test3",
                    ProviderId = 1,
                    Source = 0,
                    IsActive = true,
                    MonthlyPrice = 40M,
                    SetupPrice = 40M,
                    IsPublic = true,
                },
            };

            using var data = DatabaseMock.Instance;

            var numberService = new NumberService(data);

            data.Numbers.AddRange(numberList);
            data.SaveChanges();

            // Act
            var result = numberService.Delete(
                1,
                1,
                "0987654320",
                "test",
                10M,
                10M,
                "TestDescription",
                true,
                SourceEnum.Manual,
                DateTime.Now,
                null,
                true);

            // Assert
            Assert.Equal(3, data.Numbers.Count());
        }
    }
}
