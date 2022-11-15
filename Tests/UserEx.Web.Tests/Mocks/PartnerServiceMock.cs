namespace UserEx.Web.Tests.Mocks
{
    using Moq;
    using UserEx.Data.Models;
    using UserEx.Services.Data.Partners;
    using UserEx.Services.Data.Statistics;

    public static class PartnerServiceMock
    {
        public static IPartnerService InstancePartnerService()
        {
            var partnersServiceMock = new Mock<IPartnerService>();
            var userId = "testUseId";
            partnersServiceMock
                .Setup(s => s.IsPartner(userId))
                .Returns(true);
            return partnersServiceMock.Object;

        }
    }
}
