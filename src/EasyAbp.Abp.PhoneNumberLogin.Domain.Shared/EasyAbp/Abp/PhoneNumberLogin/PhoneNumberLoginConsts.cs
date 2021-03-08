namespace EasyAbp.Abp.PhoneNumberLogin
{
    public static class PhoneNumberLoginConsts
    {
        public static int MaxVerificationCodeLength { get; set; } = 8;

        public const string GrantType = "PhoneNumberLogin_credentials";

        public const string IdentityServerHttpClientName = "EasyAbpPhoneNumberLogin";

        public const string VerificationCodeCachePrefix = "PhoneNumberLoginVerificationCode";
    }
}