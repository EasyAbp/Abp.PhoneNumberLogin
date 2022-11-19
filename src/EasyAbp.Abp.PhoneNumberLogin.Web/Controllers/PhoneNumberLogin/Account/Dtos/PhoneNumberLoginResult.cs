using System;

namespace EasyAbp.Abp.PhoneNumberLogin.Web.Controllers.PhoneNumberLogin.Account.Dtos
{
    [Serializable]
    public class PhoneNumberLoginResult
    {
        public PhoneNumberLoginResult(PhoneNumberLoginResultType result, string errorMessage = null)
        {
            Result = result;
            ErrorMessage = errorMessage;
        }

        public PhoneNumberLoginResultType Result { get; }

        public string ErrorMessage { get; }

        public string Description => Result.ToString();
    }
}