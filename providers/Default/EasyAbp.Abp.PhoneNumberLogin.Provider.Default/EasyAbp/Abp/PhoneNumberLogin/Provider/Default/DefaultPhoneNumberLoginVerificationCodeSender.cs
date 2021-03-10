using System.Threading.Tasks;
using Volo.Abp.Settings;
using Volo.Abp.Sms;

namespace EasyAbp.Abp.PhoneNumberLogin.Provider.TencentCloud
{
    public class DefaultPhoneNumberLoginVerificationCodeSender : IPhoneNumberLoginVerificationCodeSender
    {
        private readonly ISmsSender _smsSender;


        public DefaultPhoneNumberLoginVerificationCodeSender(
            ISmsSender smsSender)
        {
            _smsSender = smsSender;
        }

        public virtual async Task<bool> SendAsync(string phoneNumber, string code, VerificationCodeType type, string message = null)
        {
            SmsMessage smsMessage = new SmsMessage(phoneNumber, code);

            await _smsSender.SendAsync(smsMessage);

            return true;
        }
    }
}