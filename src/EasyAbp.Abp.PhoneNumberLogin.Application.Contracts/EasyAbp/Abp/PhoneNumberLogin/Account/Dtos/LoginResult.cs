using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;

namespace EasyAbp.Abp.PhoneNumberLogin.Account.Dtos
{
    [Serializable]
    public class LoginResult : IMultiTenant
    {
        public string Token { get;}

        public LoginResultType Result { get; }

        public string Description => Result.ToString();

        public Guid? TenantId { get; }

        public LoginResult(LoginResultType result, string token, Guid? tenantId = null)
        {
            Result = result;
            Token = token;
            TenantId = tenantId;
        }
    }
}