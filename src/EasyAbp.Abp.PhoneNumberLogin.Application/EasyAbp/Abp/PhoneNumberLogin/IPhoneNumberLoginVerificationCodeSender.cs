using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public interface IPhoneNumberLoginVerificationCodeSender
    {
        Task<bool> SendAsync(string phoneNumber, string code, VerificationCodeType type, object textTemplateModel = null);
    }
}
