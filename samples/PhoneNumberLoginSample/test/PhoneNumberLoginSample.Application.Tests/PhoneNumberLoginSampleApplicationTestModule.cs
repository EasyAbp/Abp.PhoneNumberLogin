using Volo.Abp.Modularity;

namespace PhoneNumberLoginSample
{
    [DependsOn(
        typeof(PhoneNumberLoginSampleApplicationModule),
        typeof(PhoneNumberLoginSampleDomainTestModule)
        )]
    public class PhoneNumberLoginSampleApplicationTestModule : AbpModule
    {

    }
}