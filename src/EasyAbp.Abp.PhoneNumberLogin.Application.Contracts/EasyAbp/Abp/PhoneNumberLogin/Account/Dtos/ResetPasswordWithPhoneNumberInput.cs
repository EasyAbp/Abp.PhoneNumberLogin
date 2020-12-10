using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace EasyAbp.Abp.PhoneNumberLogin.Account.Dtos
{
    public class ResetPasswordWithPhoneNumberInput
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        [Required]
        [DynamicStringLength(typeof(PhoneNumberLoginConsts), nameof(PhoneNumberLoginConsts.MaxVerificationCodeLength))]
        public string VerificationCode { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DisableAuditing]
        public string Password { get; set; }
    }
}