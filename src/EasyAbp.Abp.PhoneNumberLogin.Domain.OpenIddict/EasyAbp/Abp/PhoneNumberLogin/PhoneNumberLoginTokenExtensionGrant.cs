using EasyAbp.Abp.PhoneNumberLogin.Identity;
using EasyAbp.Abp.PhoneNumberLogin.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public class PhoneNumberLoginTokenExtensionGrant : ITokenExtensionGrant, ITransientDependency
    {
        public string Name => PhoneNumberLoginConsts.GrantType;

        public virtual async Task<IActionResult> HandleAsync(ExtensionGrantContext context)
        {
            var identityOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<IdentityOptions>>();
            var identityUserManager = context.HttpContext.RequestServices.GetRequiredService<IdentityUserManager>();
            var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
            var scopeManager = context.HttpContext.RequestServices.GetRequiredService<IOpenIddictScopeManager>();
            var abpOpenIddictClaimsPrincipalManager = context.HttpContext.RequestServices
                .GetRequiredService<AbpOpenIddictClaimsPrincipalManager>();
            var identitySecurityLogManager =
                context.HttpContext.RequestServices.GetRequiredService<IdentitySecurityLogManager>();
            var localizer = context.HttpContext.RequestServices
                .GetRequiredService<IStringLocalizer<PhoneNumberLoginResource>>();
            var uniquePhoneNumberIdentityUserRepository = context.HttpContext.RequestServices
                .GetRequiredService<IUniquePhoneNumberIdentityUserRepository>();

            await identityOptions.SetAsync();

            var phoneNumber = context.Request.GetParameter("phone_number")?.ToString();
            var code = context.Request.GetParameter("code").ToString();
            var password = context.Request.GetParameter("password").ToString();

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return new ForbidResult(
                    new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                            OpenIddictConstants.Errors.InvalidRequest,

                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            localizer[PhoneNumberLoginErrorCodes.InvalidPhoneNumberOrPassword]
                    }!));
            }

            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrEmpty(password))
            {
                return new ForbidResult(
                    new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                            OpenIddictConstants.Errors.InvalidRequest,

                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            localizer[PhoneNumberLoginErrorCodes.InvalidCredential]
                    }!));
            }

            var identityUser =
                await uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(phoneNumber);

            if (password.IsNullOrWhiteSpace())
            {
                var result = await identityUserManager.VerifyUserTokenAsync(identityUser,
                    TokenOptions.DefaultPhoneProvider, PhoneNumberLoginConsts.LoginPurposeName, code);

                if (!result)
                {
                    return new ForbidResult(
                        new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                                OpenIddictConstants.Errors.InvalidGrant,

                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                localizer[PhoneNumberLoginErrorCodes.InvalidVerificationCode]
                        }!));
                }
            }
            else
            {
                bool result = await identityUserManager.CheckPasswordAsync(identityUser, password);

                if (!result)
                {
                    return new ForbidResult(
                        new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                                OpenIddictConstants.Errors.InvalidGrant,

                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                localizer[PhoneNumberLoginErrorCodes.InvalidPhoneNumberOrPassword]
                        }!));
                }
            }

            var principal = await signInManager.CreateUserPrincipalAsync(identityUser);

            principal.SetScopes(context.Request.GetScopes());
            principal.SetResources(await GetResourcesAsync(context.Request.GetScopes(), scopeManager));

            await abpOpenIddictClaimsPrincipalManager.HandleAsync(context.Request, principal);

            await identitySecurityLogManager.SaveAsync(
                new IdentitySecurityLogContext
                {
                    Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                    Action = OpenIddictSecurityLogActionConsts.LoginSucceeded,
                    UserName = context.Request.Username,
                    ClientId = context.Request.ClientId
                }
            );

            return new Microsoft.AspNetCore.Mvc.SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                principal);
        }

        protected virtual async Task<IEnumerable<string>> GetResourcesAsync(ImmutableArray<string> scopes,
            IOpenIddictScopeManager scopeManager)
        {
            var resources = new List<string>();
            if (!scopes.Any())
            {
                return resources;
            }

            await foreach (var resource in scopeManager.ListResourcesAsync(scopes))
            {
                resources.Add(resource);
            }

            return resources;
        }
    }
}