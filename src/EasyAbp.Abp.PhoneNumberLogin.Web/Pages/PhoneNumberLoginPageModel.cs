using EasyAbp.Abp.PhoneNumberLogin.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EasyAbp.Abp.PhoneNumberLogin.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class PhoneNumberLoginPageModel : AbpPageModel
    {
        protected PhoneNumberLoginPageModel()
        {
            LocalizationResourceType = typeof(PhoneNumberLoginResource);
            ObjectMapperContext = typeof(AbpPhoneNumberLoginWebModule);
        }
    }
}