using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class PhoneNumberLoginConsoleApiClientModule : AbpModule
    {
        
    }
}
