using PhoneNumberLoginSample.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PhoneNumberLoginSample.Permissions
{
    public class PhoneNumberLoginSamplePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(PhoneNumberLoginSamplePermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(PhoneNumberLoginSamplePermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PhoneNumberLoginSampleResource>(name);
        }
    }
}
