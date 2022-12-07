namespace UserEx.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using UserEx.Common;
    using UserEx.Data.Models;

    using static UserEx.Common.GlobalConstants;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // the seeding below is duplicated and can be removed along with the SeedRoleAsync function below

            // await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
            Task
                .Run(async () =>
                {
                    // ! initial setup
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new ApplicationRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "voice.operations@tiebreak.solutions";
                    const string adminPassword = "voip12";

                    var user = new ApplicationUser()
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Admin",
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        // private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        // {
        //    var role = await roleManager.FindByNameAsync(roleName);
        //    if (role == null)
        //    {
        //        var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
        //        if (!result.Succeeded)
        //        {
        //            throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        //        }
        //    }
        // }

        // public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        // {
        //    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    Task
        //        .Run(async () =>
        //        {
        //            if (await roleManager.RoleExistsAsync(AdministratorRoleName))
        //            {
        //                return;
        //            }
        //            var role = new ApplicationRole { Name = AdministratorRoleName };
        //            await roleManager.CreateAsync(role);
        //            const string adminEmail = "voice.operations@tiebreak.solutions";
        //            const string adminPassword = "voip12";
        //            var user = new ApplicationUser()
        //            {
        //                Email = adminEmail,
        //                UserName = adminEmail,
        //                FullName = "Admin",
        //            };
        //            await userManager.CreateAsync(user, adminPassword);
        //            await userManager.AddToRoleAsync(user, role.Name);
        //        })
        //        .GetAwaiter()
        //        .GetResult();
        // }
    }
}
