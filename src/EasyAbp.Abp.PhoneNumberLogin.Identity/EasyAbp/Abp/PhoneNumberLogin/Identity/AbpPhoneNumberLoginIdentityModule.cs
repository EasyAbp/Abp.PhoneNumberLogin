using EasyAbp.Abp.VerificationCode.Identity;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.PhoneNumberLogin.Identity
{
    [DependsOn(
        typeof(AbpVerificationCodeIdentityModule)
    )]
    public class AbpPhoneNumberLoginIdentityModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}