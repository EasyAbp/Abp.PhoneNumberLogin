using EasyAbp.Abp.PhoneNumberLogin.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace EasyAbp.Abp.PhoneNumberLogin.Settings
{
    public class PhoneNumberLoginSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(new SettingDefinition(
                PhoneNumberLoginSettings.CacheTime,
                "5",
                L("CacheTime")));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PhoneNumberLoginResource>(name);
        }
    }
}