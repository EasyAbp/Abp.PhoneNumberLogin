using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [Dependency(TryRegister = true)]
    public class DefaultPhoneLoginNewUserCreator : IPhoneNumberLoginNewUserCreator, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IdentityUserManager _identityUserManager;

        public DefaultPhoneLoginNewUserCreator(
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator,
            IOptions<IdentityOptions> identityOptions,
            IdentityUserManager identityUserManager)
        {
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _identityOptions = identityOptions;
            _identityUserManager = identityUserManager;
        }

        public virtual async Task<IdentityUser> CreateAsync(string phoneNumber, string userName = null, string password = null, string email = null)
        {
            await _identityOptions.SetAsync();

            var identityUser = new IdentityUser(_guidGenerator.Create(), userName ?? await GenerateUserNameAsync(),
                email ?? await GenerateEmailAsync(), _currentTenant.Id);

            (await _identityUserManager.CreateAsync(identityUser)).CheckErrors();

            identityUser.SetPhoneNumber(phoneNumber, true);

            if (!string.IsNullOrEmpty(password))
            {
                (await _identityUserManager.AddPasswordAsync(identityUser, password)).CheckErrors();
            }

            identityUser.Name = userName ?? phoneNumber;

            (await _identityUserManager.UpdateAsync(identityUser)).CheckErrors();

            (await _identityUserManager.AddDefaultRolesAsync(identityUser)).CheckErrors();

            return identityUser;
        }

        protected virtual Task<string> GenerateUserNameAsync()
        {
            return Task.FromResult("Phone_" + Guid.NewGuid());
        }

        protected virtual Task<string> GenerateEmailAsync()
        {
            return Task.FromResult(Guid.NewGuid() + "@fake-email.com");
        }

    }
}