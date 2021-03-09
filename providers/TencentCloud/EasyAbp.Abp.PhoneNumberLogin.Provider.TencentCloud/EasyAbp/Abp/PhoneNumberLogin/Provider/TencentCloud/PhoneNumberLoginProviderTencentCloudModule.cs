namespace EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud
{
    using EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud.Localization;
    using EasyAbp.Abp.Sms.TencentCloud;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Volo.Abp.Localization;
    using Volo.Abp.Localization.ExceptionHandling;
    using Volo.Abp.Modularity;
    using Volo.Abp.Validation.Localization;
    using Volo.Abp.VirtualFileSystem;

    [DependsOn(
        typeof(AbpSmsTencentCloudModule)
    )]
    public class PhoneNumberLoginProviderTencentCloudModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<PhoneNumberLoginProviderTencentCloudModule>();
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
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IPhoneNumberLoginVerificationCodeSender, TencentCloudPhoneNumberLoginVerificationCodeSender>());
        }
    }
}
