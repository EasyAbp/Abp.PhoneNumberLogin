using EasyAbp.Abp.PhoneNumberLogin.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(PhoneNumberLoginEntityFrameworkCoreTestModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpIdentityAspNetCoreModule)
    )]
    public class PhoneNumberLoginDomainTestModule : AbpModule
    {
        
    }
}
