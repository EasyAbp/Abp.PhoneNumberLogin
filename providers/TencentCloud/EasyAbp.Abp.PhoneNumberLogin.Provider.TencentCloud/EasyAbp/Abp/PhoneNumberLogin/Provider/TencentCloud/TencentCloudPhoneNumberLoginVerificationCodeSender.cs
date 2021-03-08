﻿namespace EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud
{
    using EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud.Settings;
    using EasyAbp.Abp.Sms.TencentCloud;
    using System.Threading.Tasks;
    using Volo.Abp.Settings;
    using Volo.Abp.Sms;
    public class TencentCloudPhoneNumberLoginVerificationCodeSender : IPhoneNumberLoginVerificationCodeSender
    {
        private readonly ISmsSender _smsSender;

        private readonly ISettingProvider _settingProvider;

        public TencentCloudPhoneNumberLoginVerificationCodeSender(
            ISmsSender smsSender,
            ISettingProvider settingProvider)
        {
            _smsSender = smsSender;
            _settingProvider = settingProvider;
        }

        public virtual async Task<bool> SendAsync(string phoneNumber, string code, VerificationCodeType type, string message = null)
        {
            string templateId = string.Empty;

            switch (type)
            {
                case VerificationCodeType.Login:
                    templateId = await _settingProvider.GetOrNullAsync(PhoneNumberLoginProviderTencentCloudSettings.LoginTemplateId);
                    break;
                case VerificationCodeType.Register:
                    templateId = await _settingProvider.GetOrNullAsync(PhoneNumberLoginProviderTencentCloudSettings.RegisterTemplateId);
                    break;
                case VerificationCodeType.Confirm:
                    templateId = await _settingProvider.GetOrNullAsync(PhoneNumberLoginProviderTencentCloudSettings.ConfirmTemplateId);
                    break;
                case VerificationCodeType.ResetPassword:
                    templateId = await _settingProvider.GetOrNullAsync(PhoneNumberLoginProviderTencentCloudSettings.ResetPasswordTemplateId);
                    break;
            }

            phoneNumber = await FormatPhoneNumberAsync(phoneNumber);

            SmsMessage smsMessage = new SmsMessage(phoneNumber, string.IsNullOrEmpty(message) ? PhoneNumberLoginProviderTencentCloudConsts.PlaceHolder : message);

            smsMessage.Properties.Add(AbpSmsTencentCloudConsts.TemplateIdPropertyName, templateId);

            smsMessage.Properties.Add(AbpSmsTencentCloudConsts.TemplateParamSetPropertyName, new[] { code });

            await _smsSender.SendAsync(smsMessage);

            return true;
        }

        protected virtual async Task<string> FormatPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.StartsWith("+"))
            {
                return phoneNumber;
            }

            var countryCode = await _settingProvider.GetOrNullAsync(PhoneNumberLoginProviderTencentCloudSettings.DefaultCountryCode) ?? "86";

            return $"+{countryCode}{phoneNumber}";
        }

    }
}