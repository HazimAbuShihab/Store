using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Store_Core7.Model
{
    public class DbInitializer
    {
        public static void Initialize(AppDBContext context)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context), null, null, null, null);
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var role = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };
                roleManager.CreateAsync(role).Wait();
            }
            if (!context.Roles.Any(r => r.Name == "User"))
            {
                var role = new IdentityRole { Name = "User", NormalizedName = "USER" };
                roleManager.CreateAsync(role).Wait();
            }
            if (!context.Roles.Any(r => r.Name == "Customer"))
            {
                var role = new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER" };
                roleManager.CreateAsync(role).Wait();
            }
        }
    }
}
