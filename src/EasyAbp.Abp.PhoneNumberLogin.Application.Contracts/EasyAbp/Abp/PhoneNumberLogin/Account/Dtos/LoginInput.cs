using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace EasyAbp.Abp.PhoneNumberLogin.Account.Dtos
{
    [Serializable]
    public class LoginInput
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }
        
        [Required]
        [DynamicStringLength(typeof(PhoneNumberLoginConsts), nameof(PhoneNumberLoginConsts.MaxVerificationCodeLength))]
        public string VerificationCode { get; set; }
        
        /// <summary>
        /// It will be generated randomly if the value is null.
        /// </summary>
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }

        /// <summary>
        /// It will be generated randomly if the value is null.
        /// </summary>
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string EmailAddress { get; set; }

        /// <summary>
        /// It will be generated randomly if the value is null.
        /// </summary>
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }
    }
}