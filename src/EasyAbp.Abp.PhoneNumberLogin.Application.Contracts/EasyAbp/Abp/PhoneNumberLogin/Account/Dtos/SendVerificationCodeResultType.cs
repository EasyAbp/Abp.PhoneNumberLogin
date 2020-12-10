namespace EasyAbp.Abp.PhoneNumberLogin.Account.Dtos
{
    public enum SendVerificationCodeResultType : byte
    {
        Success = 1,

        FrequencyIsLimited = 2,

        SendsFailure = 3
    }
}