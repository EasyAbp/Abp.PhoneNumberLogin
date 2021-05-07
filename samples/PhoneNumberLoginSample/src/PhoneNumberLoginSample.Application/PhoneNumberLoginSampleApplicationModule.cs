using EasyAbp.Abp.PhoneNumberLogin;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace PhoneNumberLoginSample
{
    [DependsOn(
        typeof(PhoneNumberLoginSampleDomainModule),
        typeof(AbpPhoneNumberLoginApplicationModule),
        typeof(AbpAccountApplicationModule),
        typeof(PhoneNumberLoginSampleApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule)
    )]
    public class PhoneNumberLoginSampleApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PhoneNumberLoginSampleApplicationModule>();
            });
        }
    }
}
