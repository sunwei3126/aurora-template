using System;
using System.Collections.Generic;
using System.Text;
using Aurora.Localization;
using Volo.Abp.Application.Services;

namespace Aurora
{
    /* Inherit your application services from this class.
     */
    public abstract class AuroraAppService : ApplicationService
    {
        protected AuroraAppService()
        {
            LocalizationResource = typeof(AuroraResource);
        }
    }
}
