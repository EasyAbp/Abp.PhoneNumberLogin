using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpPhoneNumberLoginHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "EasyAbpAbpPhoneNumberLogin";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpPhoneNumberLoginApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
