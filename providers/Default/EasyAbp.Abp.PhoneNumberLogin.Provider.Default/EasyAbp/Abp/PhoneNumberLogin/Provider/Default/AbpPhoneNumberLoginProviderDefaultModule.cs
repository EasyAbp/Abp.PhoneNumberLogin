using EasyAbp.Abp.PhoneNumberLogin.Provider.Default.Localization;
using EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.Abp.PhoneNumberLogin.Provider.Default
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginDomainModule)
    )]
    public class AbpPhoneNumberLoginProviderDefaultModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpPhoneNumberLoginProviderDefaultModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<PhoneNumberLoginProviderDefaultResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("EasyAbp/Abp/PhoneNumberLogin/Provider/Default/Localization");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud", typeof(PhoneNumberLoginProviderDefaultResource));
            });

            context.Services.Replace(ServiceDescriptor.Singleton<IPhoneNumberLoginVerificationCodeSender, DefaultPhoneNumberLoginVerificationCodeSender>());
        }
    }
}