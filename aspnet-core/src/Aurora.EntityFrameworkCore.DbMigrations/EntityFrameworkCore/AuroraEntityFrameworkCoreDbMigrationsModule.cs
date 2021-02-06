using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Aurora.EntityFrameworkCore
{
    [DependsOn(typeof(AuroraEntityFrameworkCoreModule))]
    public class AuroraEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AuroraMigrationsDbContext>();
        }
    }
}