using Aurora.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Aurora.Permissions
{
    public class AuroraPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(AuroraPermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(AuroraPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AuroraResource>(name);
        }
    }
}
