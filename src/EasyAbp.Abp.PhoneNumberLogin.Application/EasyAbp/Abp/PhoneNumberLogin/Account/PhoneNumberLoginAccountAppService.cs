using EasyAbp.Abp.PhoneNumberLogin.Account.Dtos;
using EasyAbp.Abp.PhoneNumberLogin.Identity;
using EasyAbp.Abp.PhoneNumberLogin.Settings;
using EasyAbp.Abp.VerificationCode;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
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

        public PhoneNumberLoginAccountAppService(
            IPhoneNumberLoginVerificationCodeSender phoneNumberLoginVerificationCodeSender,
            IPhoneNumberLoginNewUserCreator phoneNumberLoginNewUserCreator,
            IUniquePhoneNumberIdentityUserRepository uniquePhoneNumberIdentityUserRepository,
            IVerificationCodeManager verificationCodeManager,
            IOptions<IdentityOptions> identityOptions,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ISettingProvider settingProvider,
            IdentityUserManager identityUserManager)
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
        }

        public virtual async Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeInput input)
        {
            var identityUser = await _uniquePhoneNumberIdentityUserRepository.FindByConfirmedPhoneNumberAsync(input.PhoneNumber);

            string code = await GenerateCodeAsync(input.PhoneNumber, input.VerificationCodeType, identityUser);

            var result = await _phoneNumberLoginVerificationCodeSender.SendAsync(input.PhoneNumber, code, input.VerificationCodeType);

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
            var identityUser = await _uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(input.PhoneNumber);

            var result = await _identityUserManager.VerifyUserTokenAsync(identityUser, TokenOptions.DefaultPhoneProvider, PhoneNumberLoginConsts.ConfirmPurposeName, input.VerificationCode);

            if (!result)
            {
                return new ConfirmPhoneNumberResult(ConfirmPhoneNumberResultType.WrongVerificationCode);
            }

            identityUser.SetPhoneNumberConfirmed(true);

            (await _identityUserManager.UpdateAsync(identityUser)).CheckErrors();

            return new ConfirmPhoneNumberResult(ConfirmPhoneNumberResultType.Success);
        }

        public virtual async Task ResetPasswordAsync(ResetPasswordWithPhoneNumberInput input)
        {
            await _identityOptions.SetAsync();

            var identityUser = await _uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(input.PhoneNumber);

            (await _identityUserManager.ResetPasswordAsync(identityUser, input.VerificationCode, input.Password)).CheckErrors();
        }

        public virtual async Task<TryRegisterAndRequestTokenResult> TryRegisterAndRequestTokenAsync(LoginInput input)
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
                registerUser ? LoginResultType.Register : LoginResultType.Login,
                (await RequestIds4LoginByCodeAsync(input.PhoneNumber, code))?.Raw,
                CurrentTenant.Id);
        }

        public virtual async Task<string> RequestTokenByPasswordAsync(RequestTokenByPasswordInput input)
        {
            return (await RequestIds4LoginByPasswordAsync(input.PhoneNumber, input.Password))?.Raw;
        }

        public virtual async Task<string> RequestTokenByVerificationCodeAsync(RequestTokenByVerificationCodeInput input)
        {
            return (await RequestIds4LoginByCodeAsync(input.PhoneNumber, input.VerificationCode))?.Raw;
        }

        public virtual async Task<string> RefreshTokenAsync(RefreshTokenInput input)
        {
            return (await RequestIds4RefreshAsync(input.RefreshToken))?.Raw;
        }

        protected virtual async Task<bool> GetValidateResultAsync(string phoneNumber, string code, VerificationCodeType type)
        {
            return await _verificationCodeManager.ValidateAsync(
                codeCacheKey: $"{PhoneNumberLoginConsts.VerificationCodeCachePrefix}:{type}:{phoneNumber}",
                verificationCode: code,
                configuration: new VerificationCodeConfiguration());
        }

        protected virtual async Task<TokenResponse> RequestIds4LoginByCodeAsync(string phoneNumber, string code)
        {
            var client = _httpClientFactory.CreateClient(PhoneNumberLoginConsts.IdentityServerHttpClientName);

            var request = new TokenRequest
            {
                Address = _configuration["AuthServer:Authority"] + "/connect/token",
                GrantType = PhoneNumberLoginConsts.GrantType,

                ClientId = _configuration["AuthServer:ClientId"],
                ClientSecret = _configuration["AuthServer:ClientSecret"],

                Parameters =
                {
                    {"phonenumber", phoneNumber},
                    {"code", code}
                }
            };

            request.Headers.Add(GetTenantHeaderName(), CurrentTenant.Id?.ToString());

            return await client.RequestTokenAsync(request);
        }

        protected virtual async Task<TokenResponse> RequestIds4LoginByPasswordAsync(string phoneNumber, string password)
        {
            var client = _httpClientFactory.CreateClient(PhoneNumberLoginConsts.IdentityServerHttpClientName);

            var request = new TokenRequest
            {
                Address = _configuration["AuthServer:Authority"] + "/connect/token",
                GrantType = PhoneNumberLoginConsts.GrantType,

                ClientId = _configuration["AuthServer:ClientId"],
                ClientSecret = _configuration["AuthServer:ClientSecret"],

                Parameters =
                {
                    {"phonenumber", phoneNumber},
                    {"password", password}
                }
            };

            request.Headers.Add(GetTenantHeaderName(), CurrentTenant.Id?.ToString());

            return await client.RequestTokenAsync(request);
        }

        protected virtual async Task<TokenResponse> RequestIds4RefreshAsync(string refreshToken)
        {
            var client = _httpClientFactory.CreateClient(PhoneNumberLoginConsts.IdentityServerHttpClientName);

            var request = new RefreshTokenRequest
            {
                Address = _configuration["AuthServer:Authority"] + "/connect/token",

                ClientId = _configuration["AuthServer:ClientId"],
                ClientSecret = _configuration["AuthServer:ClientSecret"],

                RefreshToken = refreshToken
            };

            request.Headers.Add(GetTenantHeaderName(), CurrentTenant.Id?.ToString());

            return await client.RequestRefreshTokenAsync(request);
        }

        protected virtual string GetTenantHeaderName()
        {
            return "__tenant";
        }

        protected virtual async Task<int> GetCacheTime()
        {
            return await _settingProvider.GetAsync(PhoneNumberLoginSettings.RegisterCodeCacheSeconds, 5);
        }

        protected virtual async Task<string> GenerateCodeAsync(string phoneNumber, VerificationCodeType type, IdentityUser identityUser = null)
        {
            string code = string.Empty;

            switch (type)
            {
                case VerificationCodeType.ResetPassword:
                    code = await _identityUserManager.GeneratePasswordResetTokenAsync(identityUser);
                    break;

                case VerificationCodeType.Register:

                    code = await _verificationCodeManager.GenerateAsync(
                        codeCacheKey: $"{PhoneNumberLoginConsts.VerificationCodeCachePrefix}:{type}:{phoneNumber}",
                        codeCacheLifespan: TimeSpan.FromMinutes(await GetCacheTime()),
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