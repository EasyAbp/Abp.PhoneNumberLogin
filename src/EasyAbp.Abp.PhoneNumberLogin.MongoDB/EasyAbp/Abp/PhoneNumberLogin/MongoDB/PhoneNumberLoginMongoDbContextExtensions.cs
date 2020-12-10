using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace EasyAbp.Abp.PhoneNumberLogin.MongoDB
{
    public static class PhoneNumberLoginMongoDbContextExtensions
    {
        public static void ConfigureAbpPhoneNumberLogin(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PhoneNumberLoginMongoModelBuilderConfigurationOptions(
                PhoneNumberLoginDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}