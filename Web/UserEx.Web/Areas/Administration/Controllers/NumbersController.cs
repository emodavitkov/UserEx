namespace UserEx.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class NumbersController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
