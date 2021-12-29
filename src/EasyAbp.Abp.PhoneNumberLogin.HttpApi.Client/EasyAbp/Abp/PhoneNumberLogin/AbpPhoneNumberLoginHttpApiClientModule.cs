using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpPhoneNumberLoginHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = AbpPhoneNumberLoginRemoteServiceConsts.RemoteServiceName;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpPhoneNumberLoginApplicationContractsModule).Assembly,
                RemoteServiceName
            );
            
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpPhoneNumberLoginApplicationContractsModule>();
            });
        }
    }
}
