using Volo.Abp.Settings;

namespace Aurora.Settings
{
    public class AuroraSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(AuroraSettings.MySetting1));
        }
    }
}
