using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace PhoneNumberLoginSample.Web
{
    [Dependency(ReplaceServices = true)]
    public class PhoneNumberLoginSampleBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "PhoneNumberLoginSample";
    }
}
