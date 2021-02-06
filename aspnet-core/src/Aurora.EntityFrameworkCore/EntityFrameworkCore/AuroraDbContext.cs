using Aurora.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;

namespace Aurora.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class AuroraDbContext : AbpDbContext<AuroraDbContext>
    {
        public DbSet<AppUser> Users { get; set; }

        public AuroraDbContext(DbContextOptions<AuroraDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users");
                b.ConfigureByConvention();
                b.ConfigureAbpUser();
            });

            builder.ConfigureAurora();
        }
    }
}