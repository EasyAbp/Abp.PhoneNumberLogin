using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace PhoneNumberLoginSample.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(PhoneNumberLoginSampleHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class PhoneNumberLoginSampleConsoleApiClientModule : AbpModule
    {
        
    }
}
