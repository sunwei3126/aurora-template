using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Aurora.EntityFrameworkCore
{
    public static class AuroraDbContextModelCreatingExtensions
    {
        public static void ConfigureAurora(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
        }
    }
}