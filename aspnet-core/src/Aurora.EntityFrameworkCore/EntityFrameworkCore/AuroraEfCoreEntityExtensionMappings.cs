using Volo.Abp.IdentityServer;
using Volo.Abp.Threading;

namespace Aurora.EntityFrameworkCore
{
    public static class AuroraEfCoreEntityExtensionMappings
    {
        private static readonly OneTimeRunner OneTimeRunner = new();

        public static void Configure()
        {
            AuroraGlobalFeatureConfigurator.Configure();
            AuroraModuleExtensionConfigurator.Configure();

            OneTimeRunner.Run(() =>
            {
                AbpIdentityServerDbProperties.DbTablePrefix = "AbpIdentityServer";
            });
        }
    }
}