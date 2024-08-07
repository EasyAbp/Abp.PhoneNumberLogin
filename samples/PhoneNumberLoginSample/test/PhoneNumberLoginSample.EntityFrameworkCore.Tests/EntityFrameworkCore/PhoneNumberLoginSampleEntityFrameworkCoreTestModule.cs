﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace PhoneNumberLoginSample.EntityFrameworkCore
{
    [DependsOn(
        typeof(PhoneNumberLoginSampleEntityFrameworkCoreModule),
        typeof(PhoneNumberLoginSampleTestBaseModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
        )]
    public class PhoneNumberLoginSampleEntityFrameworkCoreTestModule : AbpModule
    {
        private SqliteConnection _sqliteConnection;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureInMemorySqlite(context.Services);
        }

        private void ConfigureInMemorySqlite(IServiceCollection services)
        {
            services.AddAlwaysDisableUnitOfWorkTransaction();
            _sqliteConnection = CreateDatabaseAndGetConnection();

            services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(context =>
                {
                    context.DbContextOptions.UseSqlite(_sqliteConnection);
                });
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _sqliteConnection.Dispose();
        }

        private static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new AbpUnitTestSqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<PhoneNumberLoginSampleDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new PhoneNumberLoginSampleDbContext(options))
            {
                context.GetService<IRelationalDatabaseCreator>().CreateTables();
            }

            return connection;
        }
    }
}
