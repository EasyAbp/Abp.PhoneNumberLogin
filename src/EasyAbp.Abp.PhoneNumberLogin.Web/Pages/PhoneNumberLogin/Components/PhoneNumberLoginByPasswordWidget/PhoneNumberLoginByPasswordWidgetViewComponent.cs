using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace EasyAbp.Abp.PhoneNumberLogin.Web.Pages.PhoneNumberLogin.Components.PhoneNumberLoginByPasswordWidget
{
    [ViewComponent(Name = "PhoneNumberLoginByPassword")]
    [Widget(
        ScriptTypes = new[] {typeof(PhoneNumberLoginByPasswordScriptBundleContributor)},
        StyleTypes = new[] {typeof(PhoneNumberLoginByPasswordStyleBundleContributor)}
    )]
    public class PhoneNumberLoginByPasswordWidgetViewComponent : AbpViewComponent
    {
        public virtual async Task<IViewComponentResult> InvokeAsync(PhoneNumberLoginByPasswordViewModel model)
        {
            return View("~/Pages/PhoneNumberLogin/Components/PhoneNumberLoginByPasswordWidget/Default.cshtml", model);
        }
    }
}