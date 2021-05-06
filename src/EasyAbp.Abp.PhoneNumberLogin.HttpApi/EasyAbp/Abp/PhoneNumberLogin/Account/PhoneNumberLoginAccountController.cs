using System.Threading.Tasks;
using EasyAbp.Abp.PhoneNumberLogin.Account.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Identity;

namespace EasyAbp.Abp.PhoneNumberLogin.Account
{
    [RemoteService(Name = "EasyAbpAbpPhoneNumberLogin")]
    [ControllerName("Account")]
    [Route("/api/phone-number-login/account")]
    public class PhoneNumberLoginAccountController : PhoneNumberLoginController, IPhoneNumberLoginAccountAppService
    {
        private readonly IPhoneNumberLoginAccountAppService _service;

        public PhoneNumberLoginAccountController(IPhoneNumberLoginAccountAppService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("send-verification-code")]
        public virtual Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeInput input)
        {
            return _service.SendVerificationCodeAsync(input);
        }

        [HttpPost]
        [Route("register")]
        public virtual Task<IdentityUserDto> RegisterAsync(RegisterWithPhoneNumberInput input)
        {
            return _service.RegisterAsync(input);
        }

        [HttpPost]
        [Route("confirm-phone-number")]
        public virtual Task<ConfirmPhoneNumberResult> ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input)
        {
            return _service.ConfirmPhoneNumberAsync(input);
        }

        [HttpPost]
        [Route("reset-password")]
        public virtual Task ResetPasswordAsync(ResetPasswordWithPhoneNumberInput input)
        {
            return _service.ResetPasswordAsync(input);
        }

        [HttpPost]
        [Route("request-token/by-password")]
        public virtual Task<string> RequestTokenByPasswordAsync(RequestTokenByPasswordInput input)
        {
            return _service.RequestTokenByPasswordAsync(input);
        }

        [HttpPost]
        [Route("request-token/by-verification-code")]
        public virtual Task<string> RequestTokenByVerificationCodeAsync(RequestTokenByVerificationCodeInput input)
        {
            return _service.RequestTokenByVerificationCodeAsync(input);
        }

        [HttpPost]
        [Route("refresh-token")]
        public virtual Task<string> RefreshTokenAsync(RefreshTokenInput input)
        {
            return _service.RefreshTokenAsync(input);
        }

        [HttpPost]
        [Route("register-request-token")]
        public Task<TryRegisterAndRequestTokenResult> TryRegisterAndRequestTokenAsync(TryRegisterAndRequestTokenInput input)
        {
            return _service.TryRegisterAndRequestTokenAsync(input);
        }
    }
}
