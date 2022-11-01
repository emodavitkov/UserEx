namespace UserEx.Services.Data.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data;
    using UserEx.Data.Models;

    public class NumberApiService : INumberApiService
    {
        private readonly ApplicationDbContext data;

        public NumberApiService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void Add(List<Number> numbersApiCollected)
        {
            this.data.Numbers.AddRange(numbersApiCollected);
            this.data.SaveChanges();
        }

        public bool NumberExists(string currentNumber)
        {
            // (this.data.Numbers.FirstOrDefault(n => n.DidNumber == number.Attributes.Number) != null)
            var numberExist = this.data.Numbers.FirstOrDefault(n => n.DidNumber == currentNumber);
            return numberExist != null;
        }
    }
}
