using System;

namespace EasyAbp.Abp.PhoneNumberLogin.Account.Dtos
{
    [Serializable]
    public class SendVerificationCodeResult
    {
        public SendVerificationCodeResultType Result { get; }

        public string Description => Result.ToString();
    }
}