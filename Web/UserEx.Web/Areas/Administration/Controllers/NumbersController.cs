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

        public IActionResult All([FromQuery] string filter, int currentPage = 1)
        {
            bool? select = null;
            if (filter == "all")
            {
                select = null;
            }
            else if (filter == "approve")
            {
                select = false;
            }
            else if (filter == "disable")
            {
                select = true;
            }

            var numbersQuery = this.numbers
                .All(publicOnly: select, currentPage: currentPage, numbersPerPage: AllNumbersQueryModel.NumbersPerPage);
            var result = new AllNumberServiceAdminModel
            {
                Filter = filter,
                CurrentPage = currentPage,
                TotalNumbers = numbersQuery.TotalNumbers,
                Numbers = numbersQuery.Numbers,
            };

            return this.View(result);
        }

        public IActionResult ChangeVisibility(int id, string filter)
        {
            this.numbers.ChangeVisibility(id);

            return this.RedirectToAction(nameof(this.All), new
            {
                filter =filter,
            });
        }
    }
}
