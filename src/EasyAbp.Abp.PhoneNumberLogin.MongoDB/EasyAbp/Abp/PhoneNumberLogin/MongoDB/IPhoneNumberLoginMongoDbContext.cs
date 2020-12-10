using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EasyAbp.Abp.PhoneNumberLogin.MongoDB
{
    [ConnectionStringName(PhoneNumberLoginDbProperties.ConnectionStringName)]
    public interface IPhoneNumberLoginMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
