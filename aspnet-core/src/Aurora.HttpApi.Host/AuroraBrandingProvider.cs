using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Aurora
{
    [Dependency(ReplaceServices = true)]
    public class AuroraBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Aurora";
    }
}
