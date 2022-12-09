namespace UserEx.Services.Data.Numbers
{
    using System.Collections.Generic;
    using System.Linq;

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
            var numberExist = this.data.Numbers.FirstOrDefault(n => n.DidNumber == currentNumber);
            return numberExist != null;
        }
    }
}
