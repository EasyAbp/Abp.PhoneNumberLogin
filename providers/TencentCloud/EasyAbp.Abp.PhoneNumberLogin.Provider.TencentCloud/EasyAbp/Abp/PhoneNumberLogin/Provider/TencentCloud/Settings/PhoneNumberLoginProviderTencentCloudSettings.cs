namespace EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud.Settings
{
    public static class PhoneNumberLoginProviderTencentCloudSettings
    {
        private const string Prefix = "EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud";

        //Add your own setting names here. Example:
        //public const string MySetting1 = Prefix + ".MySetting1";

        public const string RegisterTemplateId = Prefix + ".Register";
        
        public const string LoginTemplateId = Prefix + ".Login";
        
        public const string ResetPasswordTemplateId = Prefix + ".ResetPassword";
        
        public const string ConfirmTemplateId = Prefix + ".Confirm";

        public const string DefaultCountryCode = Prefix + ".DefaultCountryCode";
        
    }
}