using EasyAbp.Abp.PhoneNumberLogin.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public abstract class PhoneNumberLoginController : AbpController
    {
        protected PhoneNumberLoginController()
        {
            LocalizationResource = typeof(PhoneNumberLoginResource);
        }
    }
}
