namespace UserEx.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UserEx.Services.Data.Numbers;

    public class NumbersApiViewController : AdministrationController
    {
        private readonly INumberService numbers;
        private readonly INumberDidlogicApiService numbersDidlogic;

        public NumbersApiViewController(INumberService numbers, INumberDidlogicApiService numbersDidlogic)
        {
            this.numbers = numbers;
            this.numbersDidlogic = numbersDidlogic;
        }

        public IActionResult DidlogicApiTrigger()
        {
            var numbers = this.numbers
                .All(publicOnly: false)
                .Numbers;

            return this.RedirectToAction();
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.numbers.ChangeVisibility(id);

            return this.RedirectToAction();
        }
    }
}
