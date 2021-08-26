using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhoneNumberLoginSample.Data;
using Volo.Abp.DependencyInjection;

namespace PhoneNumberLoginSample.EntityFrameworkCore
{
    public class EntityFrameworkCorePhoneNumberLoginSampleDbSchemaMigrator
        : IPhoneNumberLoginSampleDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCorePhoneNumberLoginSampleDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the PhoneNumberLoginSampleDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<PhoneNumberLoginSampleDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
