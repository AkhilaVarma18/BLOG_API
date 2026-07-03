using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BLOG.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var ReaderRoleId = "256cbc81-c7f3-4119-8904-4e615b9ac755";
            var WriterRoleId = "bc589087-20ce-4774-9e03-398dcb2101f9";
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id=ReaderRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper(),
                    ConcurrencyStamp=ReaderRoleId
                },
                new IdentityRole()
                {
                    Id=WriterRoleId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper(),
                    ConcurrencyStamp=WriterRoleId
                },

            };
            builder.Entity<IdentityRole>().HasData(roles);
            var adminUserId = "e7f675f4-e0c3-47a0-a5f3-28528a110013";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@blog.com",
                Email = "admin@blog.com",
                NormalizedEmail = "admin@blog.com".ToUpper(),
                NormalizedUserName = "admin@blog.com".ToUpper(),

            };
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            builder.Entity<IdentityUser>().HasData(admin);
            //Give Roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>()

            {

                new()

                {

                    UserId = adminUserId,

                    RoleId = ReaderRoleId

                },

                new()

                {

                    UserId = adminUserId,

                    RoleId = WriterRoleId
                }

            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }

    }

}
