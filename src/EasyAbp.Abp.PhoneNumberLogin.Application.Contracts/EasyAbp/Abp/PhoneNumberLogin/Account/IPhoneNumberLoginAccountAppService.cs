using System.Threading.Tasks;
using EasyAbp.Abp.PhoneNumberLogin.Account.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace EasyAbp.Abp.PhoneNumberLogin.Account
{
    public interface IPhoneNumberLoginAccountAppService : IApplicationService
    {
        Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeInput input);

        Task<IdentityUserDto> RegisterAsync(RegisterWithPhoneNumberDto input);
        
        Task<ConfirmPhoneNumberResult> ConfirmPhoneNumberAsync(ConfirmPhoneNumberInput input);
        
        Task ResetPasswordAsync(ResetPasswordWithPhoneNumber input);

        Task<string> RequestTokenByPasswordAsync(RequestTokenByPassword input);

        Task<string> RequestTokenByVerificationCodeAsync(RequestTokenByVerificationCode input);

        Task<string> RefreshTokenAsync(RefreshTokenInput input);
    }
}