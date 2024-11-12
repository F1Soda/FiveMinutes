using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;

namespace FiveMinutes.Data
{
    public class Seed
    { 
        public static Task AddRoles(IApplicationBuilder applicationBuilder)
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

        public static void AddUser(IApplicationBuilder applicationBuilder, string email, string name, string password, string role)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var adminUser = userManager.FindByEmailAsync(email);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = name,
                        Email = email,
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(newAdminUser, password);
                    userManager.AddToRoleAsync(newAdminUser, role);
                }
            }
        }

        public static void SeedUsersDefailt(IApplicationBuilder applicationBuilder)
        {
            AddRoles(applicationBuilder);
            AddUser(applicationBuilder, "golik.tima@gmail.com", "golikdev", "Coding@1234?", UserRoles.Admin);
            AddUser(applicationBuilder, "ivan.ivanovich@yandex.ru", "Иван Иванович", "123456", UserRoles.Student);
            AddUser(applicationBuilder, "michail.zukov@kontur.ru", "Михаил Зюков", "123456", UserRoles.Student);
            AddUser(applicationBuilder, "maria.filatova@mail.ru", "Мария Филатова", "123456", UserRoles.Teacher);
        }
    }
}
