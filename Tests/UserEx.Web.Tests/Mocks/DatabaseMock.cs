namespace UserEx.Web.Tests.Mocks
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using UserEx.Data;

    public static class DatabaseMock
    {
        public static ApplicationDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
                return new ApplicationDbContext(dbContextOptions);
            }
        }
    }
}
