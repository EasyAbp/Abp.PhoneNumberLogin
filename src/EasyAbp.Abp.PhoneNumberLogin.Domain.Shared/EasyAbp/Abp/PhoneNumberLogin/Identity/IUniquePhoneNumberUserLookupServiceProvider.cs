using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Users;

namespace EasyAbp.Abp.PhoneNumberLogin.Identity
{
    public interface IUniquePhoneNumberUserLookupServiceProvider
    {
        Task<IUserData> FindByConfirmedPhoneNumberAsync([NotNull] string phoneNumber, CancellationToken cancellationToken = default);
    }
}