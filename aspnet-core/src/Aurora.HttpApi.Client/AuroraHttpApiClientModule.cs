using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Aurora
{
    [DependsOn(
        typeof(AuroraApplicationContractsModule),
        typeof(AbpHttpClientModule)
    )]
    public class AuroraHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Aurora";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(AuroraApplicationContractsModule).Assembly, RemoteServiceName);
        }
    }
}