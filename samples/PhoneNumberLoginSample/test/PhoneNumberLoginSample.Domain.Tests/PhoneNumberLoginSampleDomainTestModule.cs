using PhoneNumberLoginSample.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace PhoneNumberLoginSample
{
    [DependsOn(
        typeof(PhoneNumberLoginSampleEntityFrameworkCoreTestModule)
        )]
    public class PhoneNumberLoginSampleDomainTestModule : AbpModule
    {

    }
}