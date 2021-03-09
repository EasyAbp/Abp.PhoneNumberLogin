using EasyAbp.Abp.PhoneNumberLogin.Identity;
using EasyAbp.Abp.VerificationCode.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpVerificationCodeIdentityModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPhoneNumberLoginDomainSharedModule)
    )]
    public class AbpPhoneNumberLoginDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<IIdentityServerBuilder>(builder =>
            {
                builder.AddExtensionGrantValidator<PhoneNumberGrantValidator>();
            });
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.TryAddTransient<IPhoneNumberLoginNewUserCreator, DefaultPhoneLoginNewUserCreator>();
            context.Services.TryAddTransient<UniquePhoneNumberUserValidator>();
            context.Services.AddTransient<IUserValidator<IdentityUser>, UniquePhoneNumberUserValidator>();

        }
    }
}
