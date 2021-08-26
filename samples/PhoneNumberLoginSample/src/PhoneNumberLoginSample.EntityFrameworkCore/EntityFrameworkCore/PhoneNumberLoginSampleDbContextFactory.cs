using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PhoneNumberLoginSample.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class PhoneNumberLoginSampleDbContextFactory : IDesignTimeDbContextFactory<PhoneNumberLoginSampleDbContext>
    {
        public PhoneNumberLoginSampleDbContext CreateDbContext(string[] args)
        {
            PhoneNumberLoginSampleEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<PhoneNumberLoginSampleDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new PhoneNumberLoginSampleDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PhoneNumberLoginSample.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
