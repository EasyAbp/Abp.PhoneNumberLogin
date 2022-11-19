using EasyAbp.Abp.VerificationCode;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.Http.Client.IdentityModel;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginDomainModule),
        typeof(AbpPhoneNumberLoginApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpVerificationCodeModule),
        typeof(AbpHttpClientIdentityModelModule)
    )]
    public class AbpPhoneNumberLoginApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpPhoneNumberLoginApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AbpPhoneNumberLoginApplicationModule>(validate: true);
            });
        }
    }
}
