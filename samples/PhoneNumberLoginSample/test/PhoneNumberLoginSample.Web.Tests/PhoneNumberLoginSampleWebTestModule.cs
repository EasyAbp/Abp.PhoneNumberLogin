using System.Collections.Generic;
using System.Globalization;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using PhoneNumberLoginSample.EntityFrameworkCore;
using PhoneNumberLoginSample.Localization;
using PhoneNumberLoginSample.Web;
using PhoneNumberLoginSample.Web.Menus;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Validation.Localization;

namespace PhoneNumberLoginSample
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(PhoneNumberLoginSampleWebModule),
        typeof(PhoneNumberLoginSampleApplicationTestModule),
        typeof(PhoneNumberLoginSampleEntityFrameworkCoreTestModule)
    )]
    public class PhoneNumberLoginSampleWebTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<IMvcBuilder>(builder =>
            {
                builder.PartManager.ApplicationParts.Add(new CompiledRazorAssemblyPart(typeof(PhoneNumberLoginSampleWebModule).Assembly));
            });

            context.Services.GetPreConfigureActions<OpenIddictServerBuilder>().Clear();
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = true;
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalizationServices(context.Services);
            ConfigureNavigationServices(context.Services);
        }

        private static void ConfigureLocalizationServices(IServiceCollection services)
        {
            var cultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("tr") };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PhoneNumberLoginSampleResource>()
                    .AddBaseTypes(
                        typeof(AbpValidationResource),
                        typeof(AbpUiResource)
                    );
            });
        }

        private static void ConfigureNavigationServices(IServiceCollection services)
        {
            services.Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new PhoneNumberLoginSampleMenuContributor());
            });
        }
    }
}
