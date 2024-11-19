using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;

namespace FiveMinutes.Data
{
    public class Seed
    { 
        public static async Task AddRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Student))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));
                if (!await roleManager.RoleExistsAsync(UserRoles.Teacher))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Teacher));
            }
        }

        public static async Task AddUser(IApplicationBuilder applicationBuilder, string email, string name, string password, string role)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var adminUser = await userManager.FindByEmailAsync(email);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = name,
                        Email = email,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, password);
                    await userManager.AddToRoleAsync(newAdminUser, role);
                }
            }
        }
    }
}
