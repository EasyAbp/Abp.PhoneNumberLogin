using EasyAbp.Abp.VerificationCode;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.Http.Client.IdentityModel;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginDomainModule),
        typeof(AbpPhoneNumberLoginApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpMapperlyModule),
        typeof(AbpVerificationCodeModule),
        typeof(AbpHttpClientIdentityModelModule)
    )]
    public class AbpPhoneNumberLoginApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMapperlyObjectMapper<AbpPhoneNumberLoginApplicationModule>();
        }
    }
}
