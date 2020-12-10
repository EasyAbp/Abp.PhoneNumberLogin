using Volo.Abp.Modularity;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginApplicationModule),
        typeof(PhoneNumberLoginDomainTestModule)
        )]
    public class PhoneNumberLoginApplicationTestModule : AbpModule
    {

    }
}
