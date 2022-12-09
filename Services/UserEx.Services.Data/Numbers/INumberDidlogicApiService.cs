namespace UserEx.Services.Data.Numbers
{
    using System.Collections.Generic;

    using UserEx.Data.Models;

    public interface INumberDidlogicApiService
    {
        public void Add(List<Number> numbersApiCollected);

        public bool NumberExists(string currentNumber);
    }
}
