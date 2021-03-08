using Volo.Abp;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public class InvalidVerificationCodeException : UserFriendlyException
    {
        public InvalidVerificationCodeException() : base("验证码不正确", "InvalidVerificationCode")
        {
            
        }
    }
}