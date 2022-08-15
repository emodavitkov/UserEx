namespace UserEx.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using UserEx.Data.Models;

    internal class ProvidersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Providers.Any())
            {
                return;
            }

            await dbContext.Providers.AddRangeAsync(new[]
            {
                new Provider { Name = "Bezeq" },
                new Provider { Name = "CommPeak" },
                new Provider { Name = "DIDlogic" },
                new Provider { Name = "IDT" },
                new Provider { Name = "Omega" },
                new Provider { Name = "ReshetCalls" },
                new Provider { Name = "SquareTalk" },
                new Provider { Name = "Synch" },
            });
    }
    }
}
