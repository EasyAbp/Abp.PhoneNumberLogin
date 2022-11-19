using System.IO;
using EasyAbp.Abp.PhoneNumberLogin;
using EasyAbp.Abp.PhoneNumberLogin.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneNumberLoginSample.EntityFrameworkCore;
using PhoneNumberLoginSample.Localization;
using PhoneNumberLoginSample.MultiTenancy;
using PhoneNumberLoginSample.Web.Menus;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace PhoneNumberLoginSample.Web
{
    [DependsOn(
        typeof(PhoneNumberLoginSampleHttpApiModule),
        typeof(PhoneNumberLoginSampleApplicationModule),
        typeof(PhoneNumberLoginSampleEntityFrameworkCoreModule),
        typeof(AbpPhoneNumberLoginWebModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpAccountWebOpenIddictModule),
        typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
        typeof(AbpTenantManagementWebModule),
        typeof(AbpFeatureManagementWebModule),
        typeof(AbpSettingManagementWebModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
    )]
    public class PhoneNumberLoginSampleWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<OpenIddictBuilder>(builder =>
            {
                builder.AddValidation(options =>
                {
                    options.AddAudiences("PhoneNumberLoginSample"); // Replace with your application name
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
            });

            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(PhoneNumberLoginSampleResource),
                    typeof(PhoneNumberLoginSampleDomainModule).Assembly,
                    typeof(PhoneNumberLoginSampleDomainSharedModule).Assembly,
                    typeof(PhoneNumberLoginSampleApplicationModule).Assembly,
                    typeof(PhoneNumberLoginSampleApplicationContractsModule).Assembly,
                    typeof(PhoneNumberLoginSampleWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigureAuthentication(context);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureNavigationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context)
        {
            context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults
                .AuthenticationScheme);
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<PhoneNumberLoginSampleWebModule>(); });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<PhoneNumberLoginSampleDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}PhoneNumberLoginSample.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PhoneNumberLoginSampleDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}PhoneNumberLoginSample.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PhoneNumberLoginSampleApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}PhoneNumberLoginSample.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PhoneNumberLoginSampleApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}PhoneNumberLoginSample.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PhoneNumberLoginSampleWebModule>(hostingEnvironment
                        .ContentRootPath);

                    options.FileSets.ReplaceEmbeddedByPhysical<AbpPhoneNumberLoginDomainSharedModule>(Path.Combine(
                        hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.Abp.PhoneNumberLogin.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpPhoneNumberLoginDomainModule>(Path.Combine(
                        hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.Abp.PhoneNumberLogin.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpPhoneNumberLoginApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.Abp.PhoneNumberLogin.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpPhoneNumberLoginApplicationModule>(Path.Combine(
                        hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.Abp.PhoneNumberLogin.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpPhoneNumberLoginWebModule>(Path.Combine(
                        hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.Abp.PhoneNumberLogin.Web"));
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });
        }

        private void ConfigureNavigationServices()
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new PhoneNumberLoginSampleMenuContributor());
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(PhoneNumberLoginSampleApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PhoneNumberLoginSample API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseAbpOpenIddictValidation();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PhoneNumberLoginSample API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}