using Volo.Abp;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public class InvalidVerificationCodeException : BusinessException
    {
        public InvalidVerificationCodeException() : base(PhoneNumberLoginErrorCodes.InvalidVerificationCode)
        {
            
        }
    }
}