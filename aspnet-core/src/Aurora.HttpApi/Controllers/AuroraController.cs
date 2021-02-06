using Aurora.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Aurora.Controllers
{
    public abstract class AuroraController : AbpController
    {
        protected AuroraController()
        {
            LocalizationResource = typeof(AuroraResource);
        }
    }
}