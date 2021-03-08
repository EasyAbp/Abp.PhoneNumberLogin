using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud
{
    public class PhoneNumberLoginProviderTencentCloudOptions
    {
        public string RegisterTemplateId { get; set; }
        public string LoginTemplateId { get; set; }
        public string ResetPasswordTemplateId { get; set; }
        public string ConfirmTemplateId { get; set; }
    }
}
