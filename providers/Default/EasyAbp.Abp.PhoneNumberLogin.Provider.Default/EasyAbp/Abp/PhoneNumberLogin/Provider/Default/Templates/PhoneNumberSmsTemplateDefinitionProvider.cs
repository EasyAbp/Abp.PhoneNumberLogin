using System;
using Volo.Abp.TextTemplating;

namespace EasyAbp.Abp.PhoneNumberLogin.Provider.Default.Templates
{
    public class PhoneNumberSmsTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            foreach (var type in Enum.GetNames<VerificationCodeType>())
            {
                context.Add(
                    new TemplateDefinition(
                            name: $"PhoneNumberLoginSmsText_{type}",
                            defaultCultureName: "en")
                        .WithVirtualFilePath(
                            $"/EasyAbp/Abp/PhoneNumberLogin/Provider/Default/Templates/PhoneNumberLoginSmsText_{type}",
                            isInlineLocalized: false
                        )
                );
            }
        }
    }
}