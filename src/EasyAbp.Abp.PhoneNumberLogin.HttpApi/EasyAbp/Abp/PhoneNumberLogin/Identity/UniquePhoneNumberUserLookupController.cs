using Volo.Abp.Users;
using System.Threading.Tasks;
using Asp.Versioning;
using EasyAbp.Abp.PhoneNumberLogin.Account;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace EasyAbp.Abp.PhoneNumberLogin.Identity
{
    [RemoteService(Name = AbpPhoneNumberLoginRemoteServiceConsts.RemoteServiceName)]
    [ControllerName("IdentityUserLookup")]
    [Route("/api/identity/users/lookup")]
    public class UniquePhoneNumberUserLookupController : PhoneNumberLoginController, IUniquePhoneNumberUserLookupAppService
    {
        private readonly IUniquePhoneNumberUserLookupAppService _service;

        public UniquePhoneNumberUserLookupController(IUniquePhoneNumberUserLookupAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("by-confirmed-phone-number/{phoneNumber}")]
        public virtual Task<IUserData> FindByConfirmedPhoneNumberAsync(string phoneNumber)
        {
            return _service.FindByConfirmedPhoneNumberAsync(phoneNumber);
        }
    }
}