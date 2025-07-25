﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;

namespace PhoneNumberLoginSample.Web.Pages.Account
{
    public class CustomLoginModel : LoginModel
    {
        public const string UserNameMethodName = "UserName";
        public const string PhoneNumberMethodName = "PhoneNumber";

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Method { get; set; } = PhoneNumberMethodName;

        public CustomLoginModel(IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions, IOptions<IdentityOptions> identityOptions,
            IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
            IWebHostEnvironment webHostEnvironment) : base(schemeProvider, accountOptions,
            identityOptions, identityDynamicClaimsPrincipalContributorCache, webHostEnvironment)
        {
        }
    }
}