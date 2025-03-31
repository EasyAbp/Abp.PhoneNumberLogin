using EasyAbp.Abp.PhoneNumberLogin.Account.Dtos;
using EasyAbp.Abp.PhoneNumberLogin.Identity;
using EasyAbp.Abp.PhoneNumberLogin.Localization;
using EasyAbp.Abp.PhoneNumberLogin.Settings;
using EasyAbp.Abp.VerificationCode;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace EasyAbp.Abp.PhoneNumberLogin.Account
{
    public class PhoneNumberLoginAccountAppService : ApplicationService, IPhoneNumberLoginAccountAppService
    {
        private readonly IPhoneNumberLoginVerificationCodeSender _phoneNumberLoginVerificationCodeSender;
        private readonly IPhoneNumberLoginNewUserCreator _phoneNumberLoginNewUserCreator;
        private readonly IVerificationCodeManager _verificationCodeManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IUniquePhoneNumberIdentityUserRepository _uniquePhoneNumberIdentityUserRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ISettingProvider _settingProvider;
        private readonly IDistributedCache<string> _distributedCache;
        private readonly IStringLocalizer<PhoneNumberLoginResource> _localizer;

        public PhoneNumberLoginAccountAppService(
            IPhoneNumberLoginVerificationCodeSender phoneNumberLoginVerificationCodeSender,
            IPhoneNumberLoginNewUserCreator phoneNumberLoginNewUserCreator,
            IUniquePhoneNumberIdentityUserRepository uniquePhoneNumberIdentityUserRepository,
            IVerificationCodeManager verificationCodeManager,
            IOptions<IdentityOptions> identityOptions,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ISettingProvider settingProvider,
            IdentityUserManager identityUserManager,
            IDistributedCache<string> distributedCache,
            IStringLocalizer<PhoneNumberLoginResource> localizer)
        {
            _phoneNumberLoginVerificationCodeSender = phoneNumberLoginVerificationCodeSender;
            _phoneNumberLoginNewUserCreator = phoneNumberLoginNewUserCreator;
            _verificationCodeManager = verificationCodeManager;
            _uniquePhoneNumberIdentityUserRepository = uniquePhoneNumberIdentityUserRepository;
            _identityOptions = identityOptions;
            _identityUserManager = identityUserManager;
            _httpClientFactory = httpClientFactory;
            _settingProvider = settingProvider;
            _configuration = configuration;
            _distributedCache = distributedCache;
            _localizer = localizer;
        }

        public virtual async Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeInput input)
        {
            var identityUser = await _uniquePhoneNumberIdentityUserRepository.FindByConfirmedPhoneNumberAsync(input.PhoneNumber);

            string code = await GenerateCodeAsync(input.PhoneNumber, input.VerificationCodeType, identityUser);

            var result = await _phoneNumberLoginVerificationCodeSender.SendAsync(input.PhoneNumber, code,
                input.VerificationCodeType,
                new { LifespanMinutes = Math.Floor(await GetRegisterCodeCacheSecondsAsync() / 60f) });

            return result ? new SendVerificationCodeResult(SendVerificationCodeResultType.Success) : new SendVerificationCodeResult(SendVerificationCodeResultType.SendsFailure);
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterWithPhoneNumberInput input)
        {
            var result = await GetValidateResultAsync(input.PhoneNumber, input.VerificationCode, VerificationCodeType.Register);

            if (!result)
            {
                throw new InvalidVerificationCodeException();
            }

            var identityUser = await _phoneNumberLoginNewUserCreator.CreateAsync(input.PhoneNumber, input.UserName, input.Password, input.EmailAddress);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(identityUser);
        }

        public virtual async Task<ConfirmPhoneNumberResult> ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input)
        {
            var result = await GetValidateResultAsync(input.PhoneNumber, input.VerificationCode, VerificationCodeType.Register);

            if (!result)
            {
                return new ConfirmPhoneNumberResult(ConfirmPhoneNumberResultType.WrongVerificationCode);
            }

            var identityUser = await _uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(input.PhoneNumber);

            identityUser.SetPhoneNumberConfirmed(true);

            (await _identityUserManager.UpdateAsync(identityUser)).CheckErrors();

            return new ConfirmPhoneNumberResult(ConfirmPhoneNumberResultType.Success);
        }

        public virtual async Task ResetPasswordAsync(ResetPasswordWithPhoneNumberInput input)
        {
            var result = await GetValidateResultAsync(input.PhoneNumber, input.VerificationCode, VerificationCodeType.ResetPassword);
            if (!result)
            {
                throw new InvalidVerificationCodeException();
            }
            await _identityOptions.SetAsync();

            var identityUser = await _uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(input.PhoneNumber);
            // VerifyTwoFactor
            if (!await _identityUserManager.VerifyTwoFactorTokenAsync(identityUser, TokenOptions.DefaultPhoneProvider, input.VerificationCode))
            {
                throw new UserFriendlyException(_localizer["InvalidVerificationCode"]);
            }
            var resetPwdToken = await _identityUserManager.GeneratePasswordResetTokenAsync(identityUser);
            (await _identityUserManager.ResetPasswordAsync(identityUser, resetPwdToken, input.Password)).CheckErrors();
        }

        public virtual async Task<TryRegisterAndRequestTokenResult> TryRegisterAndRequestTokenAsync(TryRegisterAndRequestTokenInput input)
        {
            var result = await GetValidateResultAsync(input.PhoneNumber, input.VerificationCode, VerificationCodeType.Register);

            if (!result)
            {
                throw new InvalidVerificationCodeException();
            }

            await _identityOptions.SetAsync();

            bool registerUser = false;

            var identityUser =
                await _uniquePhoneNumberIdentityUserRepository.FindByConfirmedPhoneNumberAsync(input.PhoneNumber);

            if (identityUser is null)
            {
                using (var uow = UnitOfWorkManager.Begin(new AbpUnitOfWorkOptions(true), true))
                {
                    identityUser = await _phoneNumberLoginNewUserCreator.CreateAsync(input.PhoneNumber, input.UserName, input.Password, input.EmailAddress);

                    await uow.CompleteAsync();
                }

                registerUser = true;
            }

            string code = await GenerateCodeAsync(input.PhoneNumber, VerificationCodeType.Login, identityUser);

            return new TryRegisterAndRequestTokenResult(
                registerUser ? RegisterResult.RegistrationSuccess : RegisterResult.UserAlreadyExists,
                (await RequestAuthServerLoginByCodeAsync(input.PhoneNumber, code))?.Raw,
                CurrentTenant.Id);
        }

        public virtual async Task<string> RequestTokenByPasswordAsync(RequestTokenByPasswordInput input)
        {
            return (await RequestAuthServerLoginByPasswordAsync(input.PhoneNumber, input.Password))?.Raw;
        }

        public virtual async Task<string> RequestTokenByVerificationCodeAsync(RequestTokenByVerificationCodeInput input)
        {
            return (await RequestAuthServerLoginByCodeAsync(input.PhoneNumber, input.VerificationCode))?.Raw;
        }

        public virtual async Task<string> RefreshTokenAsync(RefreshTokenInput input)
        {
            return (await RequestAuthServerRefreshAsync(input.RefreshToken))?.Raw;
        }

        protected virtual async Task<bool> GetValidateResultAsync(string phoneNumber, string code, VerificationCodeType type)
        {
            switch (type)
            {
                case VerificationCodeType.ResetPassword:

                    var tempCode = await _distributedCache.GetAsync($"{PhoneNumberLoginConsts.VerificationCodeCachePrefix}:{type}:{phoneNumber}");
                    return tempCode.Equals(code);

                case VerificationCodeType.Register:

                    return await _verificationCodeManager.ValidateAsync(
                        codeCacheKey: $"{PhoneNumberLoginConsts.VerificationCodeCachePrefix}:{type}:{phoneNumber}",
                        verificationCode: code,
                        configuration: new VerificationCodeConfiguration());

                case VerificationCodeType.Login:

                    var loginUser = await _uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(phoneNumber);

                    return await _identityUserManager.VerifyUserTokenAsync(loginUser, TokenOptions.DefaultPhoneProvider, PhoneNumberLoginConsts.LoginPurposeName, code);

                case VerificationCodeType.Confirm:

                    var confirmPhoneUser = await _uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(phoneNumber);

                    return await _identityUserManager.VerifyUserTokenAsync(confirmPhoneUser, TokenOptions.DefaultPhoneProvider, PhoneNumberLoginConsts.ConfirmPurposeName, code);
            }

            return false;
        }

        protected virtual async Task<TokenResponse> RequestAuthServerLoginByCodeAsync(string phoneNumber, string code)
        {
            var client = _httpClientFactory.CreateClient(PhoneNumberLoginConsts.IdentityServerHttpClientName);

            var request = new TokenRequest
            {
                Address = $"{_configuration["AbpPhoneNumberLogin:AuthServer:Authority"]}/connect/token",
                GrantType = PhoneNumberLoginConsts.GrantType,

                ClientId = _configuration["AbpPhoneNumberLogin:AuthServer:ClientId"],
                ClientSecret = _configuration["AbpPhoneNumberLogin:AuthServer:ClientSecret"],

                Parameters =
                {
                    { "phone_number", phoneNumber },
                    { "code", code }
                }
            };

            request.Headers.Add(GetTenantHeaderName(), CurrentTenant.Id?.ToString());

            return await client.RequestTokenAsync(request);
        }

        protected virtual async Task<TokenResponse> RequestAuthServerLoginByPasswordAsync(string phoneNumber, string password)
        {
            var client = _httpClientFactory.CreateClient(PhoneNumberLoginConsts.IdentityServerHttpClientName);

            var request = new TokenRequest
            {
                Address = $"{_configuration["AbpPhoneNumberLogin:AuthServer:Authority"]}/connect/token",
                GrantType = PhoneNumberLoginConsts.GrantType,

                ClientId = _configuration["AbpPhoneNumberLogin:AuthServer:ClientId"],
                ClientSecret = _configuration["AbpPhoneNumberLogin:AuthServer:ClientSecret"],

                Parameters =
                {
                    { "phone_number", phoneNumber },
                    { "password", password }
                }
            };

            request.Headers.Add(GetTenantHeaderName(), CurrentTenant.Id?.ToString());

            return await client.RequestTokenAsync(request);
        }

        protected virtual async Task<TokenResponse> RequestAuthServerRefreshAsync(string refreshToken)
        {
            var client = _httpClientFactory.CreateClient(PhoneNumberLoginConsts.IdentityServerHttpClientName);

            var request = new RefreshTokenRequest
            {
                Address = $"{_configuration["AbpPhoneNumberLogin:AuthServer:Authority"]}/connect/token",

                ClientId = _configuration["AbpPhoneNumberLogin:AuthServer:ClientId"],
                ClientSecret = _configuration["AbpPhoneNumberLogin:AuthServer:ClientSecret"],

                RefreshToken = refreshToken
            };

            request.Headers.Add(GetTenantHeaderName(), CurrentTenant.Id?.ToString());

            return await client.RequestRefreshTokenAsync(request);
        }

        protected virtual string GetTenantHeaderName()
        {
            return "__tenant";
        }

        protected virtual async Task<int> GetRegisterCodeCacheSecondsAsync()
        {
            return await _settingProvider.GetAsync<int>(PhoneNumberLoginSettings.RegisterCodeCacheSeconds);
        }

        protected virtual async Task<string> GenerateCodeAsync(string phoneNumber, VerificationCodeType type, IdentityUser identityUser = null)
        {
            string code = string.Empty;

            switch (type)
            {
                case VerificationCodeType.ResetPassword:
                    if (identityUser == null)
                        throw new UserFriendlyException(_localizer["InvalidPhoneNumber"]);
                    var tspan = await GetRegisterCodeCacheSecondsAsync();
                    code = await _distributedCache.GetOrAddAsync($"{PhoneNumberLoginConsts.VerificationCodeCachePrefix}:{type}:{phoneNumber}",
                                     async () =>
                                     {
                                         return await _identityUserManager.GenerateTwoFactorTokenAsync(identityUser, TokenOptions.DefaultPhoneProvider);
                                     },
                                     () => new DistributedCacheEntryOptions
                                     {
                                         AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(tspan)
                                     }
                                );
                    break;

                case VerificationCodeType.Register:

                    code = await _verificationCodeManager.GenerateAsync(
                        codeCacheKey: $"{PhoneNumberLoginConsts.VerificationCodeCachePrefix}:{type}:{phoneNumber}",
                        codeCacheLifespan: TimeSpan.FromSeconds(await GetRegisterCodeCacheSecondsAsync()),
                        configuration: new VerificationCodeConfiguration());
                    break;

                case VerificationCodeType.Login:

                    code = await _identityUserManager.GenerateUserTokenAsync(identityUser, TokenOptions.DefaultPhoneProvider, PhoneNumberLoginConsts.LoginPurposeName);
                    break;

                case VerificationCodeType.Confirm:

                    code = await _identityUserManager.GenerateUserTokenAsync(identityUser, TokenOptions.DefaultPhoneProvider, PhoneNumberLoginConsts.ConfirmPurposeName);
                    break;
            }

            return code;
        }
    }
}