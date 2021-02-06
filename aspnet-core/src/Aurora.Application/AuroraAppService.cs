using Aurora.Localization;
using Volo.Abp.Application.Services;

namespace Aurora
{
    public abstract class AuroraAppService : ApplicationService
    {
        protected AuroraAppService()
        {
            LocalizationResource = typeof(AuroraResource);
        }
    }
}