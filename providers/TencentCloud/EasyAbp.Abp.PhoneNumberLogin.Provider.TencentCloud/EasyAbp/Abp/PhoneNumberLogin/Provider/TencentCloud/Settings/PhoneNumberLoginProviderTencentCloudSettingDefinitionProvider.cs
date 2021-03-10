using EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud.Settings
{
    public class PhoneNumberLoginProviderTencentCloudSettingDefinitionProvider : SettingDefinitionProvider
    {
        private readonly PhoneNumberLoginProviderTencentCloudOptions _phoneNumberLoginProviderTencentCloudOptions;

        public PhoneNumberLoginProviderTencentCloudSettingDefinitionProvider(
            IOptions<PhoneNumberLoginProviderTencentCloudOptions> phoneNumberLoginProviderTencentCloudOptions)
        {
            _phoneNumberLoginProviderTencentCloudOptions = phoneNumberLoginProviderTencentCloudOptions.Value;
        }

        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(DenturePlusSettings.MySetting1));

            context.Add(new SettingDefinition(
                PhoneNumberLoginProviderTencentCloudSettings.LoginTemplateId,
                _phoneNumberLoginProviderTencentCloudOptions.LoginTemplateId,
                L("LoginTemplateId")));

            context.Add(new SettingDefinition(
                PhoneNumberLoginProviderTencentCloudSettings.RegisterTemplateId,
                _phoneNumberLoginProviderTencentCloudOptions.RegisterTemplateId,
                L("RegisterTemplateId")));

            context.Add(new SettingDefinition(
                PhoneNumberLoginProviderTencentCloudSettings.ConfirmTemplateId,
                _phoneNumberLoginProviderTencentCloudOptions.ConfirmTemplateId,
                L("ConfirmTemplateId")));

            context.Add(new SettingDefinition(
                PhoneNumberLoginProviderTencentCloudSettings.ResetPasswordTemplateId,
                _phoneNumberLoginProviderTencentCloudOptions.ResetPasswordTemplateId,
                L("ResetPasswordTemplateId")));

            context.Add(new SettingDefinition(
                PhoneNumberLoginProviderTencentCloudSettings.DefaultCountryCode ??
                "86",
                displayName: L("DefaultCountryCode")));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PhoneNumberLoginProviderTencentCloudResource>(name);
        }
    }
}