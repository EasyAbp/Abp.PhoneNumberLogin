using System.Threading.Tasks;
using EasyAbp.Abp.PhoneNumberLogin.Account.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace EasyAbp.Abp.PhoneNumberLogin.Account
{
    public interface IPhoneNumberLoginAccountAppService : IApplicationService
    {
        Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeInput input);

        Task<IdentityUserDto> RegisterAsync(RegisterWithPhoneNumberInput input);
        
        Task<ConfirmPhoneNumberResult> ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input);
        
        Task ResetPasswordAsync(ResetPasswordWithPhoneNumberInput input);

        Task<string> RequestTokenByPasswordAsync(RequestTokenByPasswordInput input);

        Task<string> RequestTokenByVerificationCodeAsync(RequestTokenByVerificationCodeInput input);

        Task<string> RefreshTokenAsync(RefreshTokenInput input);
    }
}