using Localization.Resources.AbpUi;
using EasyAbp.Abp.PhoneNumberLogin.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AbpPhoneNumberLoginHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpPhoneNumberLoginHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PhoneNumberLoginResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
