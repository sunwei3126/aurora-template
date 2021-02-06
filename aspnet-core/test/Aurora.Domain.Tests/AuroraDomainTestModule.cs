using Aurora.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Aurora
{
    [DependsOn(
        typeof(AuroraEntityFrameworkCoreTestModule)
        )]
    public class AuroraDomainTestModule : AbpModule
    {

    }
}