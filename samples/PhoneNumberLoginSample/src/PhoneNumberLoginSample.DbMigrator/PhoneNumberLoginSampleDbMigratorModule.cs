using PhoneNumberLoginSample.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace PhoneNumberLoginSample.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(PhoneNumberLoginSampleEntityFrameworkCoreModule),
        typeof(PhoneNumberLoginSampleApplicationContractsModule)
        )]
    public class PhoneNumberLoginSampleDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
