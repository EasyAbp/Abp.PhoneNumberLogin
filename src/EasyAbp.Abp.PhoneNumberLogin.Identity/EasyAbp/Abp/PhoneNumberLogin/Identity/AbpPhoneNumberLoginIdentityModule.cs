using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.VerificationCode.Identity
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
