using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.Abp.PhoneNumberLogin.EntityFrameworkCore
{
    [ConnectionStringName(PhoneNumberLoginDbProperties.ConnectionStringName)]
    public interface IPhoneNumberLoginDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}