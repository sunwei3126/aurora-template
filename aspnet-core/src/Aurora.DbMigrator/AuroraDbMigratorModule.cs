using Aurora.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Aurora.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AuroraEntityFrameworkCoreDbMigrationsModule),
        typeof(AuroraApplicationContractsModule)
    )]
    public class AuroraDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}