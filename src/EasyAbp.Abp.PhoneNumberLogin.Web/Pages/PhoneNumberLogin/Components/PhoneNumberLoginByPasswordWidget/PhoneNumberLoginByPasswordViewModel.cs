using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace EasyAbp.Abp.PhoneNumberLogin.Web.Pages.PhoneNumberLogin.Components.PhoneNumberLoginByPasswordWidget
{
    public class PhoneNumberLoginByPasswordViewModel
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}