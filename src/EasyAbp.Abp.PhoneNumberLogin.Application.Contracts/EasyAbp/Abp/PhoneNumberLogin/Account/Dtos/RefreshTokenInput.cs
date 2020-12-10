using System;
using System.ComponentModel.DataAnnotations;

namespace EasyAbp.Abp.PhoneNumberLogin.Account.Dtos
{
    [Serializable]
    public class RefreshTokenInput
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}