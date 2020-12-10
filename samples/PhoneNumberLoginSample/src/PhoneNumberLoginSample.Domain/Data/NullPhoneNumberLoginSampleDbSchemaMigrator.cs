using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PhoneNumberLoginSample.Data
{
    /* This is used if database provider does't define
     * IPhoneNumberLoginSampleDbSchemaMigrator implementation.
     */
    public class NullPhoneNumberLoginSampleDbSchemaMigrator : IPhoneNumberLoginSampleDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}