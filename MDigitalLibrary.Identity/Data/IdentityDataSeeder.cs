using MDigitalLibrary.Services;
using Microsoft.AspNetCore.Identity;

namespace MDigitalLibrary.Identity.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using MDigitalLibrary.Services;
    using Microsoft.AspNetCore.Identity;
    using Models;

    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityDataSeeder(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void SeedData()
        {
            if (this.roleManager.Roles.Any())
            {
                return;
            }

            Task
                .Run(async () =>
                {
                    var adminRole = new IdentityRole(Constants.AdministratorRoleName);
                    var moderatorRole = new IdentityRole(Constants.ModeratorRoleName);
                    var userRole = new IdentityRole(Constants.UserRoleName);

                    await this.roleManager.CreateAsync(adminRole);
                    await this.roleManager.CreateAsync(moderatorRole);
                    await this.roleManager.CreateAsync(userRole);

                    var adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@abv.bg",
                        SecurityStamp = "RandomSecurityStamp"
                    };

                    await userManager.CreateAsync(adminUser, "123456");

                    await userManager.AddToRoleAsync(adminUser, Constants.AdministratorRoleName);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
