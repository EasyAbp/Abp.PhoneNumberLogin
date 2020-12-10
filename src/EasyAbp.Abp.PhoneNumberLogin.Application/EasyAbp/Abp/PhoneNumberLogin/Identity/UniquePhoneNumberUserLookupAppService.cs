using System.Threading.Tasks;
using EasyAbp.Abp.PhoneNumberLogin.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace EasyAbp.Abp.PhoneNumberLogin.Identity
{
    [Authorize(PhoneNumberLoginPermissions.UserLookup.Default)]
    public class UniquePhoneNumberUserLookupAppService : ApplicationService, IUniquePhoneNumberUserLookupAppService
    {
        private readonly RepositoryUniquePhoneNumberUserLookupServiceProvider _userLookupServiceProvider;

        public UniquePhoneNumberUserLookupAppService(
            RepositoryUniquePhoneNumberUserLookupServiceProvider userLookupServiceProvider)
        {
            _userLookupServiceProvider = userLookupServiceProvider;
        }
        
        public virtual async Task<IUserData> FindByConfirmedPhoneNumberAsync(string phoneNumber)
        {
            return await _userLookupServiceProvider.FindByConfirmedPhoneNumberAsync(phoneNumber);
        }
    }
}