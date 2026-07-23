using System;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public class PhoneNumberLoginObjectMapping_Tests : PhoneNumberLoginApplicationTestBase
    {
        private readonly IObjectMapper<AbpPhoneNumberLoginApplicationModule> _objectMapper;

        public PhoneNumberLoginObjectMapping_Tests()
        {
            _objectMapper = GetRequiredService<IObjectMapper<AbpPhoneNumberLoginApplicationModule>>();
        }

        [Fact]
        public void Should_Map_IdentityUser_To_IdentityUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new IdentityUser(userId, "john", "john@example.com")
            {
                Name = "John",
                Surname = "Doe"
            };
            user.SetProperty("CustomProp", "custom-value");

            // Act
            var dto = _objectMapper.Map<IdentityUser, IdentityUserDto>(user);

            // Assert
            dto.Id.ShouldBe(userId);
            dto.UserName.ShouldBe("john");
            dto.Email.ShouldBe("john@example.com");
            dto.Name.ShouldBe("John");
            dto.Surname.ShouldBe("Doe");
            dto.GetProperty<string>("CustomProp").ShouldBe("custom-value");
        }
    }
}
