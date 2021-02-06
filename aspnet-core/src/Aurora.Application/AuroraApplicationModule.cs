using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Aurora
{
    [DependsOn(
        typeof(AuroraDomainModule),
        typeof(AuroraApplicationContractsModule)
    )]
    public class AuroraApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AuroraApplicationModule>();
            });
        }
    }
}