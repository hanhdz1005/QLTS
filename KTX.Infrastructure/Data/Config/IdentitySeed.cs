using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Data.Config
{
    public class IdentitySeed
    {
        public static async Task SeedAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {


            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));


            var admin = await userManager.FindByEmailAsync("admin@gmail.com");
            if(admin == null)
            {
                var newAdmin = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    DisplayName = "Administrator",
                    Role = "Admin",
                    Code = "ADMIN",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(newAdmin, "Admin@1234");
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
    }
}

