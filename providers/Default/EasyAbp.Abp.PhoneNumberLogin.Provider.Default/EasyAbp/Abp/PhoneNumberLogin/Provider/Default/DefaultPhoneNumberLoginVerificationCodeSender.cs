using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Sms;
using Volo.Abp.TextTemplating;

namespace EasyAbp.Abp.PhoneNumberLogin.Provider.Default
{
    public class DefaultPhoneNumberLoginVerificationCodeSender : IPhoneNumberLoginVerificationCodeSender
    {
        private readonly ITemplateRenderer _templateRenderer;
        private readonly ISmsSender _smsSender;

        public DefaultPhoneNumberLoginVerificationCodeSender(
            ITemplateRenderer templateRenderer,
            ISmsSender smsSender)
        {
            _templateRenderer = templateRenderer;
            _smsSender = smsSender;
        }

        public virtual async Task<bool> SendAsync(string phoneNumber, string code, VerificationCodeType type, object textTemplateModel = null)
        {
            var text = await _templateRenderer.RenderAsync(
                templateName: $"PhoneNumberLoginSmsText_{type}",
                model: textTemplateModel,
                globalContext: new Dictionary<string, object>
                {
                    {"phoneNumber", phoneNumber},
                    {"code", code}
                });

            var smsMessage = new SmsMessage(phoneNumber, text);

            await _smsSender.SendAsync(smsMessage);

            return true;
        }
    }
}