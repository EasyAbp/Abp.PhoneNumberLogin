using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public class NullPhoneNumberLoginVerificationCodeSender : IPhoneNumberLoginVerificationCodeSender, ITransientDependency
    {
        public Task<bool> SendAsync(string phoneNumber, string code, VerificationCodeType type, object textTemplateModel = null)
        {
            return Task.FromResult(false);
        }
    }
}
