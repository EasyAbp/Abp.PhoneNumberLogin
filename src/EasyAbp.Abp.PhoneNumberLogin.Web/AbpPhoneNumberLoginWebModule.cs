using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using EasyAbp.Abp.PhoneNumberLogin.Localization;
using EasyAbp.Abp.PhoneNumberLogin.Web.Menus;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Identity;

namespace EasyAbp.Abp.PhoneNumberLogin.Web
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginDomainModule),
        typeof(AbpPhoneNumberLoginHttpApiModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAccountWebModule),
        typeof(AbpIdentityDomainSharedModule),
        typeof(AbpAutoMapperModule)
    )]
    public class AbpPhoneNumberLoginWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(PhoneNumberLoginResource), typeof(AbpPhoneNumberLoginWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpPhoneNumberLoginWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new PhoneNumberLoginMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpPhoneNumberLoginWebModule>();
            });

            context.Services.AddAutoMapperObjectMapper<AbpPhoneNumberLoginWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AbpPhoneNumberLoginWebModule>(validate: true);
            });

            Configure<RazorPagesOptions>(options =>
            {
                //Configure authorization.
            });
        }
    }
}
