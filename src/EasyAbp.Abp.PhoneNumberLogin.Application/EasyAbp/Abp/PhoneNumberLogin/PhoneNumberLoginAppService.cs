using EasyAbp.Abp.PhoneNumberLogin.Localization;
using Volo.Abp.Application.Services;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public abstract class PhoneNumberLoginAppService : ApplicationService
    {
        protected PhoneNumberLoginAppService()
        {
            LocalizationResource = typeof(PhoneNumberLoginResource);
            ObjectMapperContext = typeof(AbpPhoneNumberLoginApplicationModule);
        }
    }
}
