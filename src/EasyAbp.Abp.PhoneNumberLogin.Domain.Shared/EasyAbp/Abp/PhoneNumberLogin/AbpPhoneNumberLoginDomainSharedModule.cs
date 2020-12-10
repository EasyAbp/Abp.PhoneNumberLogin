using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using EasyAbp.Abp.PhoneNumberLogin.Localization;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Users;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpValidationModule)
    )]
    public class AbpPhoneNumberLoginDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpPhoneNumberLoginDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<PhoneNumberLoginResource>("en")
                    .AddBaseTypes(typeof(AccountResource))
                    .AddVirtualJson("/EasyAbp/Abp/PhoneNumberLogin/Localization");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("EasyAbp.Abp.PhoneNumberLogin", typeof(PhoneNumberLoginResource));
            });
        }
    }
}
