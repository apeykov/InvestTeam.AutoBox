using InvestTeam.AutoBox.Infrastructure.Persistence;
using InvestTeam.AutoBox.SelfService.Web.API.Context;
using InvestTeam.AutoBox.SelfService.Web.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace SelfService.Web.API
{
    public static class IdentitySeed
    {

        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }
        }

        public static void SeedUserStore(this IApplicationBuilder app)
        {
            SeedStore(app).GetAwaiter().GetResult();
        }

        private async static Task SeedStore(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                IConfiguration config =
                    scope.ServiceProvider.GetService<IConfiguration>();
                UserManager<User> userManager =
                    scope.ServiceProvider.GetService<UserManager<User>>();
                RoleManager<IdentityRole> roleManager =
                    scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                string roleNameManager = config["AppRole:Role"] ?? "ManagerTest1_1_2_02";
                string roleNameMechanic = config["AppRole:Role"] ?? "MechanicTest2";
                string userName = config["AppMainAccount:User"] ?? "admin@test.com_1";
                string password = config["AppMainAccount:Password"] ?? "St@n!2345";

                if (!await roleManager.RoleExistsAsync(roleNameManager))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleNameManager));
                }
                if (!await roleManager.RoleExistsAsync(roleNameMechanic)) 
                {
                    await roleManager.CreateAsync(new IdentityRole(roleNameMechanic));
                }
                User appAdminUser =
                    await userManager.FindByEmailAsync(userName);
                if (appAdminUser == null)
                {
                    appAdminUser = new User
                    {
                        UserName = userName,
                        Email = userName,
                        Name = "Main Admin Account",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(appAdminUser);
                    appAdminUser = await userManager.FindByEmailAsync(userName);
                    await userManager.AddPasswordAsync(appAdminUser, password);
                }
                if (!await userManager.IsInRoleAsync(appAdminUser, roleNameManager))
                {
                    await userManager.AddToRoleAsync(appAdminUser, roleNameManager);
                }
            }
        }
    }
}
