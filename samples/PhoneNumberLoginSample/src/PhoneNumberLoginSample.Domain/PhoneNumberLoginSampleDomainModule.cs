using EasyAbp.Abp.PhoneNumberLogin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PhoneNumberLoginSample.MultiTenancy;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace PhoneNumberLoginSample
{
    [DependsOn(
        typeof(PhoneNumberLoginSampleDomainSharedModule),
        typeof(AbpPhoneNumberLoginDomainModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AbpEmailingModule)
    )]
    public class PhoneNumberLoginSampleDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
            
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
            context.Services.AddTransient<PhoneNumberTokenProvider<IdentityUser>>();
            
            Configure<IdentityOptions>(options =>
            {
                options.Tokens.ProviderMap.TryAdd(TokenOptions.DefaultPhoneProvider,
                    new TokenProviderDescriptor(typeof(PhoneNumberTokenProvider<IdentityUser>)));
            });
        }
    }
}
