using Aurora.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Aurora.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class AuroraController : AbpController
    {
        protected AuroraController()
        {
            LocalizationResource = typeof(AuroraResource);
        }
    }
}