using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace Aurora
{
    [DependsOn(
        typeof(AuroraDomainSharedModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class AuroraApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AuroraDtoExtensions.Configure();
        }
    }
}