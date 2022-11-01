namespace UserEx.Services.Data.Numbers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UserEx.Data.Models;

    public interface INumberApiService
    {
        public void Add(List<Number> numbersApiCollected);

        public bool NumberExists(string currentNumber);
    }
}
