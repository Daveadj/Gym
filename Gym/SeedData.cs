using Microsoft.AspNetCore.Identity;

namespace Gym
{
    public static class SeedData
    {
        public static void Seed(RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var isSuperAdminRolePresent = roleManager.RoleExistsAsync("Super Administrator").Result;
            if (!isSuperAdminRolePresent)
            {
                var role = new IdentityRole
                {
                    Name = "Super Administrator"
                };
                roleManager.CreateAsync(role).Wait();
            }

            var isAdminRolePresent = roleManager.RoleExistsAsync("Administrator").Result;
            if (!isAdminRolePresent)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                roleManager.CreateAsync(role).Wait();
            }

            var isUserRolePresent = roleManager.RoleExistsAsync("User").Result;
            if (!isUserRolePresent)
            {
                var role = new IdentityRole
                {
                    Name = "User"
                };
                roleManager.CreateAsync(role).Wait();
            }
        }
    }
}