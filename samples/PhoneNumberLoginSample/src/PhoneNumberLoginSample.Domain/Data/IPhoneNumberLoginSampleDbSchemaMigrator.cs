using System.Threading.Tasks;

namespace PhoneNumberLoginSample.Data
{
    public interface IPhoneNumberLoginSampleDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
