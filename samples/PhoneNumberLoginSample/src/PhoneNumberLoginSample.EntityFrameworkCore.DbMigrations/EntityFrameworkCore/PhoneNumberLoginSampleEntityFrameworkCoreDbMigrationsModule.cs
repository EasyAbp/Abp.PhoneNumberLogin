using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace PhoneNumberLoginSample.EntityFrameworkCore
{
    [DependsOn(
        typeof(PhoneNumberLoginSampleEntityFrameworkCoreModule)
        )]
    public class PhoneNumberLoginSampleEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PhoneNumberLoginSampleMigrationsDbContext>();
        }
    }
}
