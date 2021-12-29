using EasyAbp.Abp.PhoneNumberLogin.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [Area(AbpPhoneNumberLoginRemoteServiceConsts.ModuleName)]
    public abstract class PhoneNumberLoginController : AbpControllerBase
    {
        protected PhoneNumberLoginController()
        {
            LocalizationResource = typeof(PhoneNumberLoginResource);
        }
    }
}
