namespace UserEx.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UserEx.Services.Data.Numbers;

    public class NumbersController : AdministrationController
    {
        private readonly INumberService numbers;

        public NumbersController(INumberService numbers)
        {
            this.numbers = numbers;
        }

        public IActionResult All()
        {
            var numbers = this.numbers
                .All(publicOnly: false)
                .Numbers;

            return this.View(numbers);

            // return this.View(this.numbers.All(publicOnly: false).Numbers);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.numbers.ChangeVisibility(id);

            return this.RedirectToAction(nameof(this.All));
        }

    }
}
