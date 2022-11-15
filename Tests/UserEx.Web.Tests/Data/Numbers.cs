namespace UserEx.Web.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using UserEx.Data.Models;

    public static class Numbers
    {
        public static IEnumerable<Number> TenNumbers()
            => Enumerable.Range(0, 10).Select(i => new Number
            {
                IsPublic = true,
            });
    }
}
