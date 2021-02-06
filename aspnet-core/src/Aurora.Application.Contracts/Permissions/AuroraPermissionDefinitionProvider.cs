using Aurora.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Aurora.Permissions
{
    public class AuroraPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var aurora = context.AddGroup(AuroraPermissions.GroupName);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AuroraResource>(name);
        }
    }
}