using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PriceBondAPI.Models
{
    public class PbAuthDbContext : IdentityDbContext
    {
        public PbAuthDbContext(DbContextOptions<PbAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var AdminId = "e24f0eb1-478f-4b28-97d9-6a4f58f94df2";
            var UserId = "b6c2e8f9-1520-4f07-b7d3-9c0da8c5ca9a";


            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=UserId,
                    ConcurrencyStamp=UserId,
                    Name="User",
                    NormalizedName="User".ToUpper()
                },
                new IdentityRole
                {
                    Id=AdminId,
                    ConcurrencyStamp=AdminId,
                    Name="Admin",
                    NormalizedName="Admin".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}

