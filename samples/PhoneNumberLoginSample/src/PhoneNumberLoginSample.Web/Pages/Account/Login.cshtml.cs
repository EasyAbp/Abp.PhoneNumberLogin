using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;

namespace PhoneNumberLoginSample.Web.Pages.Account
{
    public class CustomLoginModel : LoginModel
    {
        public const string UserNameMethodName = "UserName";
        public const string PhoneNumberMethodName = "PhoneNumber";
        
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Method { get; set; } = PhoneNumberMethodName;

        public CustomLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IOptions<IdentityOptions> identityOptions)
            : base(schemeProvider, accountOptions, identityOptions)
        {
        }
    }
}