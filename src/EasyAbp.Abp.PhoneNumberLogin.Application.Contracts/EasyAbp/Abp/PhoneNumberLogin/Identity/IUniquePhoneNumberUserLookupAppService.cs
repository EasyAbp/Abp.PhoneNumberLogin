using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace EasyAbp.Abp.PhoneNumberLogin.Identity
{
    public interface IUniquePhoneNumberUserLookupAppService : IApplicationService
    {
        Task<IUserData> FindByConfirmedPhoneNumberAsync(string phoneNumber);
    }
}