using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace PhoneNumberLoginSample.Users
{
    public class IdentityUserDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IGuidGenerator _guidGenerator;

        public IdentityUserDataSeedContributor(
            IIdentityUserRepository identityUserRepository,
            IdentityUserManager identityUserManager,
            IGuidGenerator guidGenerator)
        {
            _identityUserRepository = identityUserRepository;
            _identityUserManager = identityUserManager;
            _guidGenerator = guidGenerator;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await CreateUserWithConfirmedPhoneNumberAsync();
        }

        private async Task CreateUserWithConfirmedPhoneNumberAsync()
        {
            var user = new IdentityUser(_guidGenerator.Create(), "test1", "test1@gmail.com");

            await _identityUserManager.CreateAsync(user, "1q2w3E*");

            const string phoneNumber = "13000000000";

            await _identityUserManager.ChangePhoneNumberAsync(user, phoneNumber,
                await _identityUserManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber));
        }
    }
}
