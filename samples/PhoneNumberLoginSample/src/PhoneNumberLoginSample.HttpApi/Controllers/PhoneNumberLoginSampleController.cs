using PhoneNumberLoginSample.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PhoneNumberLoginSample.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class PhoneNumberLoginSampleController : AbpController
    {
        protected PhoneNumberLoginSampleController()
        {
            LocalizationResource = typeof(PhoneNumberLoginSampleResource);
        }
    }
}