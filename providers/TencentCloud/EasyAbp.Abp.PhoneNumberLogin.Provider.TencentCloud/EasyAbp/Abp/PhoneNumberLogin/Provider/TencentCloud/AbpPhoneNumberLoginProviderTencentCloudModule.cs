using EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud.Localization;
using EasyAbp.Abp.Sms.TencentCloud;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
namespace EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud
{

    [DependsOn(
        typeof(AbpPhoneNumberLoginApplicationModule),
        typeof(AbpSmsTencentCloudModule)
    )]
    public class AbpPhoneNumberLoginProviderTencentCloudModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpPhoneNumberLoginProviderTencentCloudModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<PhoneNumberLoginProviderTencentCloudResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("EasyAbp/Abp/PhoneNumberLogin/Provider/TencentCloud/Localization");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud", typeof(PhoneNumberLoginProviderTencentCloudResource));
            });

            context.Services.Replace(ServiceDescriptor.Singleton<IPhoneNumberLoginVerificationCodeSender, TencentCloudPhoneNumberLoginVerificationCodeSender>());
        }
    }
}
