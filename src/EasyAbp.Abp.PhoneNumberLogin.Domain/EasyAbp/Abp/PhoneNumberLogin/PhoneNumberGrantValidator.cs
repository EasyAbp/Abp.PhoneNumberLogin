using EasyAbp.Abp.PhoneNumberLogin.Identity;
using EasyAbp.Abp.VerificationCode;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
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
    public class PhoneNumberGrantValidator : IExtensionGrantValidator, ITransientDependency
    {
        public string GrantType => PhoneNumberLoginConsts.GrantType;

        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IUniquePhoneNumberIdentityUserRepository _uniquePhoneNumberIdentityUserRepository;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IVerificationCodeManager _verificationCodeManager;
        public PhoneNumberGrantValidator(
            IVerificationCodeManager verificationCodeManager,
            IdentityUserManager identityUserManager,
            IOptions<IdentityOptions> identityOptions,
            IUniquePhoneNumberIdentityUserRepository uniquePhoneNumberIdentityUserRepository)
        {
            _verificationCodeManager = verificationCodeManager;
            _identityUserManager = identityUserManager;
            _identityOptions = identityOptions;
            _uniquePhoneNumberIdentityUserRepository = uniquePhoneNumberIdentityUserRepository;
        }

        public virtual async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            await _identityOptions.SetAsync();

            var phonenumber = context.Request.Raw.Get("phonenumber");
            var code = context.Request.Raw.Get("code");
            var password = context.Request.Raw.Get("password");

            if (string.IsNullOrWhiteSpace(phonenumber))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                {
                    ErrorDescription = "请提供有效的电话号码"
                };

                return;
            }

            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrEmpty(password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                {
                    ErrorDescription = "请提供有效的密码或验证码"
                };

                return;
            }

            var identityUser = await _uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(phonenumber);

            if (password.IsNullOrWhiteSpace())
            {
                bool result = await _verificationCodeManager.ValidateAsync(
                codeCacheKey: $"{PhoneNumberLoginConsts.VerificationCodeCachePrefix}:{VerificationCodeType.Login}:{phonenumber}",
                verificationCode: code,
                configuration: new VerificationCodeConfiguration());

                if (!result)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    {
                        ErrorDescription = "验证码无效"
                    };

                    return;
                }
            }
            else
            {
                bool result = await _identityUserManager.CheckPasswordAsync(identityUser, password);

                if (!result)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    {
                        ErrorDescription = "密码无效"
                    };

                    return;
                }
            }

            var claims = new List<Claim>
            {
                // 记录 phonenumber
                new Claim("phonenumber", phonenumber)
            };

            if (identityUser.TenantId.HasValue)
            {
                claims.Add(new Claim(AbpClaimTypes.TenantId, identityUser.TenantId?.ToString()));
            }

            claims.AddRange(identityUser.Claims.Select(item => new Claim(item.ClaimType, item.ClaimValue)));

            context.Result = new GrantValidationResult(identityUser.Id.ToString(), GrantType, claims);
        }
    }
}