using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace EasyAbp.Abp.PhoneNumberLogin.Web.Pages.PhoneNumberLogin.Components.PhoneNumberLoginByPasswordWidget
{
    public class PhoneNumberLoginByPasswordScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/Pages/PhoneNumberLogin/Components/PhoneNumberLoginByPasswordWidget/Default.js");
        }
    }
}