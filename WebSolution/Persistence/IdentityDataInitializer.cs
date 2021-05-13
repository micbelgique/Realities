using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using System.Security.Claims;

namespace Persistence
{
    public static class IdentityDataInitializer
    {
        public static void SeedData(UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            var username = $"username";
            if (userManager.FindByNameAsync(username).Result == null)
            {
                var user = new User { UserName = username, Email = "mail" , NickName = username, Name="name", Surname="admin"};
                var createUserResult = userManager.CreateAsync(user, "password").Result;
                if (createUserResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "role").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<Role> roleManager)
        {
            var roles = new List<string>()
            {
                "role",
                "role1",
                "role3"
            };

            foreach (var roleTitle in roles)
            {
                if (!roleManager.RoleExistsAsync(roleTitle).Result)
                {
                    var role = new Role { Name = roleTitle };
                    var createRoleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }
    }
}
