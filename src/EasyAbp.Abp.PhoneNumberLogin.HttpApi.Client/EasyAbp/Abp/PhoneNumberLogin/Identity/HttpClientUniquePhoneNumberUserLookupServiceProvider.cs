using System.Threading;
using System.Threading.Tasks;
using EasyAbp.Abp.PhoneNumberLogin.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace EasyAbp.Abp.PhoneNumberLogin.Identity
{
    [Dependency(TryRegister = true)]
    public class HttpClientUniquePhoneNumberUserLookupServiceProvider : IUniquePhoneNumberUserLookupServiceProvider, ITransientDependency
    {
        private readonly IUniquePhoneNumberUserLookupAppService _userLookupAppService;

        public HttpClientUniquePhoneNumberUserLookupServiceProvider(
            IUniquePhoneNumberUserLookupAppService userLookupAppService)
        {
            _userLookupAppService = userLookupAppService;
        }

        public virtual async Task<IUserData> FindByConfirmedPhoneNumberAsync(string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            return await _userLookupAppService.FindByConfirmedPhoneNumberAsync(phoneNumber);
        }
    }
}