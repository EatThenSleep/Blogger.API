using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        // writer role is admin
        // reader role is public

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "ff4e682a-94dd-43e8-bafd-cceaec160d90";
            var writerRoleId = "f6301485-34e0-46ec-a436-d1fda69ea8d9";

            // Create Reader And Writer Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToLower(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToLower(),
                    ConcurrencyStamp = writerRoleId
                },
            };

            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            // Create an Admin User
            var adminUserId = "8842457c-2323-4fd6-969e-c320b461b3d2";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "huynhvangiang0504@gmail.com",
                Email = "huynhvangiang0504@gmail.com",
                NormalizedEmail = "huynhvangiang0504@gmail.com".ToUpper(),
                NormalizedUserName = "huynhvangiang0504@gmail.com".ToUpper(),
            };
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Cong&Vien0504");

            builder.Entity<IdentityUser>().HasData(admin);

            // Give role to Admin User
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
