using Volo.Abp.Settings;

namespace EasyAbp.Abp.PhoneNumberLogin.Provider.Default.Settings
{
    public class PhoneNumberLoginProviderDefaultSettingDefinitionProvider : SettingDefinitionProvider
    {
        public PhoneNumberLoginProviderDefaultSettingDefinitionProvider()
        {
        }

        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(DenturePlusSettings.MySetting1));
        }
    }
}