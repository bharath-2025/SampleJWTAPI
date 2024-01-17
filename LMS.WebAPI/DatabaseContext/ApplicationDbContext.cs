using LMS.WebAPI.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.WebAPI.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        //Note: IdentityDbContext will offers certain predefined DbSets to manage Identity of user for authentication and authorization.
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public ApplicationDbContext() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }
}
