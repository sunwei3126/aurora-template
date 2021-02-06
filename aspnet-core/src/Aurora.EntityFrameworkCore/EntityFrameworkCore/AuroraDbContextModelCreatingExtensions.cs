using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Aurora.EntityFrameworkCore
{
    public static class AuroraDbContextModelCreatingExtensions
    {
        public static void ConfigureAurora(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(AuroraConsts.DbTablePrefix + "YourEntities", AuroraConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}