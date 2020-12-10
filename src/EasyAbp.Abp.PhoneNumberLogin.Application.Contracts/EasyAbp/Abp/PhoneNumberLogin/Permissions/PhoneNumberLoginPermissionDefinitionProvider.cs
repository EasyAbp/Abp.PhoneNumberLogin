using EasyAbp.Abp.PhoneNumberLogin.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EasyAbp.Abp.PhoneNumberLogin.Permissions
{
    public class PhoneNumberLoginPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(PhoneNumberLoginPermissions.GroupName, L("Permission:PhoneNumberLogin"));

            myGroup
                .AddPermission(PhoneNumberLoginPermissions.UserLookup.Default, L("Permission:UserLookup"))
                .WithProviders(ClientPermissionValueProvider.ProviderName);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PhoneNumberLoginResource>(name);
        }
    }
}