namespace UserEx.Web.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data.Models;
    using UserEx.Services.Data.Partners;
    using UserEx.Web.Tests.Mocks;
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

            data.Partners.Add(new Partner
            {
                UserId = userId,
                OfficeName = officeName,
                PhoneNumber = phoneNumber,
            });
            data.SaveChanges();

            var partnerService = new PartnerService(data);

            // Act
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
    }
}
