using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpOpenIddictAspNetCoreModule),
        typeof(AbpPhoneNumberLoginDomainModule)
    )]
    public class AbpPhoneNumberLoginDomainOpenIddictModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<OpenIddictServerBuilder>(builder =>
            {
                builder.Configure(openIddictServerOptions =>
                {
                    openIddictServerOptions.GrantTypes.Add(PhoneNumberLoginConsts.GrantType);
                });
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
            {
                options.Grants.Add(PhoneNumberLoginConsts.GrantType,
                    new PhoneNumberLoginTokenExtensionGrant());
            });
        }
    }
}