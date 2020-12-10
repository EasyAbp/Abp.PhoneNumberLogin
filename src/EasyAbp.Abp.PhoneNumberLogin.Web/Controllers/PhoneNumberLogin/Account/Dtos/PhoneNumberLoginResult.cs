using System;

namespace EasyAbp.Abp.PhoneNumberLogin.Web.Controllers.PhoneNumberLogin.Account.Dtos
{
    [Serializable]
    public class PhoneNumberLoginResult
    {
        public PhoneNumberLoginResult(PhoneNumberLoginResultType result)
        {
            Result = result;
        }

        public PhoneNumberLoginResultType Result { get; }

        public string Description => Result.ToString();
    }
}