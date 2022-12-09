namespace UserEx.Web.Tests.Services
{
    using System;

    using UserEx.Data.Models;
    using UserEx.Services.Data.Partners;
    using UserEx.Web.Tests.Mocks;
    using UserEx.Web.ViewModels.Partners;
    using Xunit;

    public class PartnerServiceTest
    {
        [Fact]
        public void IsPartnerShouldReturnTrueWhenUserIsPartner()
        {
            // Arrange
            const string userId = "TestUserId";
            const string officeName = "TestOffice";
            const string phoneNumber = "123456789";

            using var data = DatabaseMock.Instance;

            var partnerService = new PartnerService(data);

            // Act
            partnerService.SetUp(
                new SetUpPartnerFormModel
            {
                OfficeName = officeName,
                PhoneNumber = phoneNumber,
            },
                userId);
            var result = partnerService.IsPartner(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPartnerShouldReturnFalseWhenUserIsPartner()
        {
            // Arrange
            const string userId = "TestUserId";
            const string officeName = "TestOffice";
            const string phoneNumber = "123456789";

            using var data = DatabaseMock.Instance;

            data.Partners.Add(new Partner
            {
                UserId = userId,
                OfficeName = officeName,
                PhoneNumber = phoneNumber,
            });
            data.SaveChanges();

            var partnerService = new PartnerService(data);

            // Act
            var result = partnerService.IsPartner("AnotherUserId");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnPartnerIdByUserId()
        {
            // Arrange
            const int id = 1;
            const string userId = "TestUserId";
            const string officeName = "TestOffice";
            const string phoneNumber = "123456789";

            using var data = DatabaseMock.Instance;

            data.Partners.Add(new Partner
            {
                Id = id,
                UserId = userId,
                OfficeName = officeName,
                PhoneNumber = phoneNumber,
            });
            data.SaveChanges();

            var partnerService = new PartnerService(data);

            // Act
            var result = partnerService.GetIdByUser(userId);

            // Assert
            Assert.Equal(id,result);
        }

        [Fact]
        public void ShouldNotReturnPartnerIdByUserId()
        {
            // Arrange
            const int id = 1;
            const string userId = "TestUserId";
            const string officeName = "TestOffice";
            const string phoneNumber = "123456789";

            using var data = DatabaseMock.Instance;

            data.Partners.Add(new Partner
            {
                Id = id,
                UserId = userId,
                OfficeName = officeName,
                PhoneNumber = phoneNumber,
            });
            data.SaveChanges();

            var partnerService = new PartnerService(data);

            // Act
           // var result = partnerService.GetIdByUser("testUserId2");

            // Assert
            Assert.Throws<ArgumentNullException>(() => partnerService.GetIdByUser("testUserId2"));
        }
    }
}
