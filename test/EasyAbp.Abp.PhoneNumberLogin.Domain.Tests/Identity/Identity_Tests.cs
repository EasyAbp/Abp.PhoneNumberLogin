using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Xunit;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace EasyAbp.Abp.PhoneNumberLogin.Identity
{
    public class Identity_Tests : PhoneNumberLoginDomainTestBase
    {
        public Identity_Tests()
        {
        }

        [Fact]
        public async Task Should_Get_Error_When_Setting_A_Duplicate_PhoneNumber()
        {
            var identityUserManager = ServiceProvider.GetRequiredService<IdentityUserManager>();

            var user1 = new IdentityUser(Guid.NewGuid(), "user1", "user1@gmail.com", null);
            var user2 = new IdentityUser(Guid.NewGuid(), "user2", "user2@gmail.com", null);
            
            ShouldBeSuccessful(await identityUserManager.CreateAsync(user1));
            ShouldBeSuccessful(await identityUserManager.CreateAsync(user2));

            const string phoneNumber = "123456";

            ShouldBeSuccessful(await identityUserManager.ChangePhoneNumberAsync(user1, phoneNumber,
                await identityUserManager.GenerateChangePhoneNumberTokenAsync(user1, phoneNumber)));

            var identityResult = await identityUserManager.ChangePhoneNumberAsync(user2, phoneNumber,
                await identityUserManager.GenerateChangePhoneNumberTokenAsync(user2, phoneNumber));
            
            identityResult.Succeeded.ShouldBeFalse();
            identityResult.Errors.Select(x => x.Code)
                .ShouldContain(UniquePhoneNumberUserValidator.DuplicatePhoneNumberErrorCode);
        }
        
        [Fact]
        public async Task Should_Get_Error_When_Setting_A_Non_Numeric_PhoneNumber()
        {
            var identityUserManager = ServiceProvider.GetRequiredService<IdentityUserManager>();

            var user1 = new IdentityUser(Guid.NewGuid(), "user1", "user1@gmail.com", null);
            
            ShouldBeSuccessful(await identityUserManager.CreateAsync(user1));

            const string phoneNumber = "+123456";

            var identityResult = await identityUserManager.ChangePhoneNumberAsync(user1, phoneNumber,
                await identityUserManager.GenerateChangePhoneNumberTokenAsync(user1, phoneNumber));
            
            identityResult.Succeeded.ShouldBeFalse();
            identityResult.Errors.Select(x => x.Code)
                .ShouldContain(UniquePhoneNumberUserValidator.NonNumericPhoneNumberErrorCode);
        }
        
        [Fact]
        public async Task Should_Get_Error_When_Setting_A_PhoneNumber_Starts_With_Zero()
        {
            var identityUserManager = ServiceProvider.GetRequiredService<IdentityUserManager>();

            var user1 = new IdentityUser(Guid.NewGuid(), "user1", "user1@gmail.com", null);
            
            ShouldBeSuccessful(await identityUserManager.CreateAsync(user1));

            const string phoneNumber = "0123456";

            var identityResult = await identityUserManager.ChangePhoneNumberAsync(user1, phoneNumber,
                await identityUserManager.GenerateChangePhoneNumberTokenAsync(user1, phoneNumber));
            
            identityResult.Succeeded.ShouldBeFalse();
            identityResult.Errors.Select(x => x.Code)
                .ShouldContain(UniquePhoneNumberUserValidator.PhoneNumberStartsWithZeroErrorCode);
        }

        private static void ShouldBeSuccessful(IdentityResult identityResult)
        {
            identityResult.Succeeded.ShouldBeTrue();
            identityResult.Errors.ShouldBeEmpty();
        }
    }
}
