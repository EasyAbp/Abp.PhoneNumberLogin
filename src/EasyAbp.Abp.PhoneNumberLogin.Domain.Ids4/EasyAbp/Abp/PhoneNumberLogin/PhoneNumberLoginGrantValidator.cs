using EasyAbp.Abp.PhoneNumberLogin.Identity;
using EasyAbp.Abp.PhoneNumberLogin.Localization;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public class PhoneNumberLoginGrantValidator : IExtensionGrantValidator, ITransientDependency
    {
        public string GrantType => PhoneNumberLoginConsts.GrantType;

        private readonly IStringLocalizer<PhoneNumberLoginResource> _localizer;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IUniquePhoneNumberIdentityUserRepository _uniquePhoneNumberIdentityUserRepository;
        private readonly IdentityUserManager _identityUserManager;

        public PhoneNumberLoginGrantValidator(
            IStringLocalizer<PhoneNumberLoginResource> localizer,
            IdentityUserManager identityUserManager,
            IOptions<IdentityOptions> identityOptions,
            IUniquePhoneNumberIdentityUserRepository uniquePhoneNumberIdentityUserRepository)
        {
            _localizer = localizer;
            _identityUserManager = identityUserManager;
            _identityOptions = identityOptions;
            _uniquePhoneNumberIdentityUserRepository = uniquePhoneNumberIdentityUserRepository;
        }

        public virtual async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            await _identityOptions.SetAsync();

            var phoneNumber = context.Request.Raw.Get("phone_number");
            var code = context.Request.Raw.Get("code");
            var password = context.Request.Raw.Get("password");

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest)
                {
                    ErrorDescription = _localizer[PhoneNumberLoginErrorCodes.InvalidPhoneNumberOrPassword]
                };

                return;
            }

            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrEmpty(password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest)
                {
                    ErrorDescription = _localizer[PhoneNumberLoginErrorCodes.InvalidCredential]
                };

                return;
            }

            var identityUser =
                await _uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(phoneNumber);

            if (password.IsNullOrWhiteSpace())
            {
                var result = await _identityUserManager.VerifyUserTokenAsync(identityUser,
                    TokenOptions.DefaultPhoneProvider, PhoneNumberLoginConsts.LoginPurposeName, code);

                if (!result)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    {
                        ErrorDescription = _localizer[PhoneNumberLoginErrorCodes.InvalidVerificationCode]
                    };

                    return;
                }
            }
            else
            {
                var result = await _identityUserManager.CheckPasswordAsync(identityUser, password);

                if (!result)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    {
                        ErrorDescription = _localizer[PhoneNumberLoginErrorCodes.InvalidPhoneNumberOrPassword]
                    };

                    return;
                }
            }

            var claims = new List<Claim>
            {
                // record phonenumber
                new("phonenumber", phoneNumber), // use "phone_number" instead.
            };

            if (identityUser.TenantId.HasValue)
            {
                claims.Add(new Claim(AbpClaimTypes.TenantId, identityUser.TenantId?.ToString() ?? string.Empty));
            }

            claims.AddRange(identityUser.Claims.Select(item => new Claim(item.ClaimType, item.ClaimValue)));

            context.Result = new GrantValidationResult(identityUser.Id.ToString(), GrantType, claims);
        }
    }
}