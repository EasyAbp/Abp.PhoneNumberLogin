using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.Abp.PhoneNumberLogin.EntityFrameworkCore
{
    public class PhoneNumberLoginModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public PhoneNumberLoginModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}