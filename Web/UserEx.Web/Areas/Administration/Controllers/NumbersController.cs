namespace UserEx.Web.Areas.Administration.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using UserEx.Services.Data.Numbers;
    using UserEx.Services.Data.Numbers.Models;
    using UserEx.Web.ViewModels.Numbers;

    public class NumbersController : AdministrationController
    {
        private readonly INumberService numbers;

        public NumbersController(INumberService numbers)
        {
            this.numbers = numbers;
        }

        public IActionResult All([FromQuery] int currentPage = 1)
        {
            //var numbers = this.numbers
            //    .All(publicOnly: false)
            //    .Numbers;

            var numbersQuery = this.numbers
                .All(publicOnly: false, currentPage: currentPage, numbersPerPage: AllNumbersQueryModel.NumbersPerPage);

            var result = new AllNumberServiceAdminModel
            {
                CurrentPage =currentPage,
                TotalNumbers = numbersQuery.TotalNumbers,
                Numbers = numbersQuery.Numbers,
            };

            return this.View(result);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.numbers.ChangeVisibility(id);

            return this.RedirectToAction(nameof(this.All));
        }

    }
}
