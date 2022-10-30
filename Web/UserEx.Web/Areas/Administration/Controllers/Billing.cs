namespace UserEx.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class Billing : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
