using Aurora.Localization;
using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Aurora
{
    [DependsOn(
        typeof(AuroraApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class AuroraHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Get<AuroraResource>().AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}