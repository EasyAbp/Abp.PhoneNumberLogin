using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace PhoneNumberLoginSample.Web
{
    [Dependency(ReplaceServices = true)]
    public class PhoneNumberLoginSampleBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "PhoneNumberLoginSample";
    }
}
