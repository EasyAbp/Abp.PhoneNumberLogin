using System.Threading.Tasks;
using EasyAbp.Abp.PhoneNumberLogin.Account.Dtos;
using IdentityModel.Client;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace EasyAbp.Abp.PhoneNumberLogin.Account
{
    public class PhoneNumberLoginAccountAppService : ApplicationService, IPhoneNumberLoginAccountAppService
    {
        public virtual Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeInput input)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<IdentityUserDto> RegisterAsync(RegisterWithPhoneNumberDto input)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<ConfirmPhoneNumberResult> ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task ResetPasswordAsync(ResetPasswordWithPhoneNumber input)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<string> RequestTokenByPasswordAsync(RequestTokenByPassword input)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<string> RequestTokenByVerificationCodeAsync(RequestTokenByVerificationCode input)
        {
            throw new System.NotImplementedException();
        }

        public virtual async Task<string> RefreshTokenAsync(RefreshTokenInput input)
        {
            return (await RequestIds4RefreshAsync(input.RefreshToken))?.Raw;
        }
        
        protected virtual Task<TokenResponse> RequestIds4LoginAsync(string appId, string unionId, string openId)
        {
            throw new System.NotImplementedException();
        }
        
        protected virtual Task<TokenResponse> RequestIds4RefreshAsync(string refreshToken)
        {
            throw new System.NotImplementedException();
        }
    }
}