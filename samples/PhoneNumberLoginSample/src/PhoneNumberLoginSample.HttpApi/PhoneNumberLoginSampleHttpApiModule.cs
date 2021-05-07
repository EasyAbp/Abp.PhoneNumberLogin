using EasyAbp.Abp.PhoneNumberLogin;
using Localization.Resources.AbpUi;
using PhoneNumberLoginSample.Localization;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace PhoneNumberLoginSample
{
    [DependsOn(
        typeof(PhoneNumberLoginSampleApplicationContractsModule),
        typeof(AbpPhoneNumberLoginHttpApiModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule)
    )]
    public class PhoneNumberLoginSampleHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PhoneNumberLoginSampleResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
