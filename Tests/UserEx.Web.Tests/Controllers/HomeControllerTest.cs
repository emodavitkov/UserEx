namespace UserEx.Web.Tests.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using MyTested.AspNetCore.Mvc;
    using UserEx.Data.Models;
    using UserEx.Services.Data.Numbers.Models;
    using UserEx.Web.Controllers;
    using UserEx.Web.ViewModels.Home;
    using Xunit;

    using static UserEx.Web.Tests.Data.Numbers;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which(controller => controller
                    .WithData(TenNumbers()))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<NumberIndexViewModel>>());

        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View();
    }
}
