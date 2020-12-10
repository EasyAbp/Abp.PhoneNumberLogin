using System;

namespace EasyAbp.Abp.PhoneNumberLogin.Account.Dtos
{
    [Serializable]
    public class ConfirmPhoneNumberResult
    {
        public ConfirmPhoneNumberResultType Result { get; }

        public string Description => Result.ToString();
    }
}