namespace UserEx.Services.Data.Numbers
{
    using System.Collections.Generic;

    using UserEx.Data.Models;

    public interface INumberApiService
    {
        public void Add(List<Number> numbersApiCollected);

        public bool NumberExists(string currentNumber);
    }
}
