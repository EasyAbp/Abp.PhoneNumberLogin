using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.PhoneNumberLogin.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Volo.Abp.TextTemplating;
using Xunit;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace EasyAbp.Abp.PhoneNumberLogin.Provider.Default.Tests.Tests
{
    public class Sms_Tests : PhoneNumberLoginTestBase<PhoneNumberLoginTestBaseModule>
    {
        public Sms_Tests()
        {
        }

        [Fact]
        public async Task Should_Render_Sms_Text()
        {
            var templateRenderer = ServiceProvider.GetRequiredService<ITemplateRenderer>();
            
            const string phoneNumber = "13000000000";
            const string code = "123456";
            
            var text = await templateRenderer.RenderAsync(
                templateName: "PhoneNumberLoginSmsText_Register",
                model: new {LifespanMinutes = Convert.ToInt32(Math.Floor(await GetRegisterCodeCacheSecondsAsync() / 60f))},
                cultureName: "en",
                globalContext: new Dictionary<string, object>
                {
                    {"phoneNumber", phoneNumber},
                    {"code", code}
                });
            
            text.ShouldBe($"Your dynamic code is {code}. It is valid for 5 minutes. Do not provide it to anyone.");
        }

        private static Task<int> GetRegisterCodeCacheSecondsAsync()
        {
            return Task.FromResult(301);
        }
    }
}
