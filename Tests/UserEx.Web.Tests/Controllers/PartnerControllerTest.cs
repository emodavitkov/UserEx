using UserEx.Web.Controllers;

namespace UserEx.Web.Tests.Controllers
{
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class PartnerControllerTest
    {
        [Fact]
        public void RouteTest()
            => MyRouting
                .Configuration()
                .ShouldMap("/Partners/SetUp")
                .To<PartnersController>(c => c.SetUp());

        [Fact]
        public void SetUpShouldBeForAuthorizedUsersAndReturnView()
            => MyController<PartnersController>
                .Instance()
                .Calling(c => c.SetUp())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
    }
}
