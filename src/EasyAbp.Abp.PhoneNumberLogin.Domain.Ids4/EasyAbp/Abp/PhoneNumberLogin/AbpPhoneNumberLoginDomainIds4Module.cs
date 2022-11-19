using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPhoneNumberLoginDomainModule)
    )]
    public class AbpPhoneNumberLoginDomainIds4Module : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<IIdentityServerBuilder>(builder =>
            {
                builder.AddExtensionGrantValidator<PhoneNumberLoginGrantValidator>();
            });
        }
    }
}