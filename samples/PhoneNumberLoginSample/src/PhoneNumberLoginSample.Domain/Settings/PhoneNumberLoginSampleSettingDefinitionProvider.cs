using Volo.Abp.Settings;

namespace PhoneNumberLoginSample.Settings
{
    public class PhoneNumberLoginSampleSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(PhoneNumberLoginSampleSettings.MySetting1));
        }
    }
}
