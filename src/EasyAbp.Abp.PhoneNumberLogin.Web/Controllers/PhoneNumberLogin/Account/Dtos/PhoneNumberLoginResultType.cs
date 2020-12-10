namespace EasyAbp.Abp.PhoneNumberLogin.Web.Controllers.PhoneNumberLogin.Account.Dtos
{
    public enum PhoneNumberLoginResultType : byte
    {
        Success = 1,

        InvalidPhoneNumberOrPassword = 2,

        NotAllowed = 3,

        LockedOut = 4,

        RequiresTwoFactor = 5
    }
}