using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace EasyAbp.Abp.PhoneNumberLogin.MongoDB
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class AbpPhoneNumberLoginMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<PhoneNumberLoginMongoDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
            });
        }
    }
}
