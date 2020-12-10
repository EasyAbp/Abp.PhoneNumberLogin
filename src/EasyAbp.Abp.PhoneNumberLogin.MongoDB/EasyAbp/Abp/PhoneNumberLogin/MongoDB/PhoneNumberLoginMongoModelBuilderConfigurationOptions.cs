using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace EasyAbp.Abp.PhoneNumberLogin.MongoDB
{
    public class PhoneNumberLoginMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public PhoneNumberLoginMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}