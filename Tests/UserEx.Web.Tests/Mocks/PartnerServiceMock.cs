namespace UserEx.Web.Tests.Mocks
{
    using Moq;
    using UserEx.Services.Data.Partners;

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
