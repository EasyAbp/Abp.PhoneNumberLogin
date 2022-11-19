using System;
using System.Threading.Tasks;
using EasyAbp.Abp.PhoneNumberLogin.Identity;
using EasyAbp.Abp.PhoneNumberLogin.Localization;
using EasyAbp.Abp.PhoneNumberLogin.Web.Controllers.PhoneNumberLogin.Account.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Settings;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EasyAbp.Abp.PhoneNumberLogin.Web.Controllers.PhoneNumberLogin.Account
{
    [Route("/api/phone-number-login/account")]
    public class LoginController : AbpControllerBase
    {
        private readonly ISettingProvider _settingProvider;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentitySecurityLogManager _identitySecurityLogManager;
        private readonly IUniquePhoneNumberIdentityUserRepository _identityUserRepository;

        public LoginController(
            ISettingProvider settingProvider,
            SignInManager<IdentityUser> signInManager,
            IdentitySecurityLogManager identitySecurityLogManager,
            IUniquePhoneNumberIdentityUserRepository identityUserRepository)
        {
            LocalizationResource = typeof(PhoneNumberLoginResource);

            _settingProvider = settingProvider;
            _signInManager = signInManager;
            _identitySecurityLogManager = identitySecurityLogManager;
            _identityUserRepository = identityUserRepository;
        }

        [HttpPost]
        [Route("login/by-password")]
        public virtual async Task<PhoneNumberLoginResult> LoginByPasswordAsync(LoginByPasswordInput input)
        {
            await CheckLocalLoginAsync();

            var user = await _identityUserRepository.FindByConfirmedPhoneNumberAsync(input.PhoneNumber);

            if (user == null)
            {
                return new PhoneNumberLoginResult(PhoneNumberLoginResultType.InvalidPhoneNumberOrPassword,
                    L[PhoneNumberLoginErrorCodes.InvalidPhoneNumberOrPassword]);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
                user.UserName,
                input.Password,
                input.RememberMe,
                true
            );

            await _identitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = signInResult.ToIdentitySecurityLogAction(),
                UserName = user.UserName
            });

            return GetLoginResult(signInResult);
        }

        [HttpPost]
        [Route("login/by-verification-code")]
        public virtual Task<PhoneNumberLoginResult> LoginByVerificationCodeAsync(LoginByVerificationCodeInput input)
        {
            throw new NotImplementedException();
        }

        protected virtual PhoneNumberLoginResult GetLoginResult(SignInResult result)
        {
            if (result.IsLockedOut)
            {
                return new PhoneNumberLoginResult(PhoneNumberLoginResultType.LockedOut, L["UserLockedOutMessage"]);
            }

            if (result.RequiresTwoFactor)
            {
                return new PhoneNumberLoginResult(PhoneNumberLoginResultType.RequiresTwoFactor, L["RequiresTwoFactor"]);
            }

            if (result.IsNotAllowed)
            {
                return new PhoneNumberLoginResult(PhoneNumberLoginResultType.NotAllowed, L["LoginIsNotAllowed"]);
            }

            if (!result.Succeeded)
            {
                return new PhoneNumberLoginResult(PhoneNumberLoginResultType.InvalidPhoneNumberOrPassword,
                    L[PhoneNumberLoginErrorCodes.InvalidPhoneNumberOrPassword]);
            }

            return new PhoneNumberLoginResult(PhoneNumberLoginResultType.Success);
        }

        protected virtual async Task CheckLocalLoginAsync()
        {
            if (!await _settingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["LocalLoginDisabledMessage"]);
            }
        }
    }
}