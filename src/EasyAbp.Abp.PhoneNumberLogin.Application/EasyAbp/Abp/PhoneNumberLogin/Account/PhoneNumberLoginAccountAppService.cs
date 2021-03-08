using System;
using System.Net.Http;
using System.Threading.Tasks;
using EasyAbp.Abp.PhoneNumberLogin.Account.Dtos;
using EasyAbp.Abp.PhoneNumberLogin.Identity;
using EasyAbp.Abp.VerificationCode;
using IdentityModel.Client;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Microsoft.AspNetCore.Identity;

namespace EasyAbp.Abp.PhoneNumberLogin.Account
{
    public class PhoneNumberLoginAccountAppService : ApplicationService, IPhoneNumberLoginAccountAppService
    {
        /// <summary>
        /// EasyAbpNotification 目前不支持没有用户的时候发送通知，所以先直接用 ISmsSender 发送短信
        /// </summary>
        private readonly IPhoneNumberLoginVerificationCodeSender _phoneNumberLoginVerificationCodeSender;
        private readonly IPhoneNumberLoginNewUserCreator _phoneNumberLoginNewUserCreator;
        private readonly IVerificationCodeManager _verificationCodeManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IUniquePhoneNumberIdentityUserRepository _uniquePhoneNumberIdentityUserRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public PhoneNumberLoginAccountAppService(
            IPhoneNumberLoginVerificationCodeSender phoneNumberLoginVerificationCodeSender,
            IPhoneNumberLoginNewUserCreator phoneNumberLoginNewUserCreator,
            IUniquePhoneNumberIdentityUserRepository uniquePhoneNumberIdentityUserRepository,
            IVerificationCodeManager verificationCodeManager,
            IOptions<IdentityOptions> identityOptions,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IdentityUserManager identityUserManager)
        {
            _phoneNumberLoginVerificationCodeSender = phoneNumberLoginVerificationCodeSender;
            _phoneNumberLoginNewUserCreator = phoneNumberLoginNewUserCreator;
            _verificationCodeManager = verificationCodeManager;
            _uniquePhoneNumberIdentityUserRepository = uniquePhoneNumberIdentityUserRepository;
            _identityOptions = identityOptions;
            _identityUserManager = identityUserManager;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        public virtual async Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeInput input)
        {
            var code = await _verificationCodeManager.GenerateAsync(
                codeCacheKey: $"{PhoneNumberLoginConsts.VerificationCodeCachePrefix}:{input.VerificationCodeType}:{input.PhoneNumber}",
                codeCacheLifespan: TimeSpan.FromMinutes(3),
                configuration: new VerificationCodeConfiguration());

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
            var result = await _verificationCodeManager.ValidateAsync(
                codeCacheKey: $"{PhoneNumberLoginConsts.VerificationCodeCachePrefix}:{VerificationCodeType.Confirm}:{input.PhoneNumber}",
                verificationCode: input.VerificationCode,
                configuration: new VerificationCodeConfiguration());

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

            var identityUser = await _uniquePhoneNumberIdentityUserRepository.GetByConfirmedPhoneNumberAsync(input.PhoneNumber);

            (await _identityUserManager.ResetPasswordAsync(identityUser, input.VerificationCode, input.Password)).CheckErrors();

        }

        public virtual async Task<string> RequestTokenByPasswordAsync(RequestTokenByPasswordInput input)
        {
            return (await RequestIds4LoginByCodeAsync(input.PhoneNumber, input.Password))?.Raw;
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
    }
}