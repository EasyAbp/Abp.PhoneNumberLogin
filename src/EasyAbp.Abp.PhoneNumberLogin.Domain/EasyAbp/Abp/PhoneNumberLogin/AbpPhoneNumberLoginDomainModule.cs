using EasyAbp.Abp.PhoneNumberLogin.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPhoneNumberLoginDomainSharedModule)
    )]
    public class AbpPhoneNumberLoginDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.TryAddTransient<IPhoneNumberLoginNewUserCreator, DefaultPhoneLoginNewUserCreator>();
            context.Services.TryAddTransient<UniquePhoneNumberUserValidator>();
            context.Services.AddTransient<IUserValidator<IdentityUser>, UniquePhoneNumberUserValidator>();
        }
    }
}