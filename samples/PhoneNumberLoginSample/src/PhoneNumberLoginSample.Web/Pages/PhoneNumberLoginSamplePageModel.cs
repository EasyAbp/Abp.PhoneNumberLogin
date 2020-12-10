using PhoneNumberLoginSample.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace PhoneNumberLoginSample.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class PhoneNumberLoginSamplePageModel : AbpPageModel
    {
        protected PhoneNumberLoginSamplePageModel()
        {
            LocalizationResourceType = typeof(PhoneNumberLoginSampleResource);
        }
    }
}