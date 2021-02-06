using Volo.Abp.Modularity;

namespace Aurora
{
    [DependsOn(
        typeof(AuroraApplicationModule),
        typeof(AuroraDomainTestModule)
    )]
    public class AuroraApplicationTestModule : AbpModule
    {
    }
}