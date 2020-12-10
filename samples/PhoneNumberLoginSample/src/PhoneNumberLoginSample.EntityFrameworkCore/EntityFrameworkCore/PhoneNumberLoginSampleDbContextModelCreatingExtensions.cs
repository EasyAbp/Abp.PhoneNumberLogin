using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace PhoneNumberLoginSample.EntityFrameworkCore
{
    public static class PhoneNumberLoginSampleDbContextModelCreatingExtensions
    {
        public static void ConfigurePhoneNumberLoginSample(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(PhoneNumberLoginSampleConsts.DbTablePrefix + "YourEntities", PhoneNumberLoginSampleConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}