using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace PhoneNumberLoginSample.Identity
{
    public class UserWithPhoneNumberDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IdentityUserManager _userManager;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public UserWithPhoneNumberDataSeedContributor(
            IOptions<IdentityOptions> identityOptions,
            IIdentityUserRepository userRepository,
            IdentityUserManager userManager,
            ILookupNormalizer lookupNormalizer,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _identityOptions = identityOptions;
            _userRepository = userRepository;
            _userManager = userManager;
            _lookupNormalizer = lookupNormalizer;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }
        
        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context.TenantId))
            {
                await _identityOptions.SetAsync();

                const string userName = "testMan";
                const string userEmail = "test@man.com";
                const string userPhoneNumber = "13000000000";
                const string userPassword = "1q2w3E*";
                
                var user = await _userRepository.FindByNormalizedUserNameAsync(
                    _lookupNormalizer.NormalizeName(userName)
                );

                if (user != null)
                {
                    return;
                }

                user = new IdentityUser(
                    _guidGenerator.Create(),
                    userName,
                    userEmail,
                    context.TenantId
                )
                {
                    Name = userName
                };

                user.SetPhoneNumber(userPhoneNumber, true);

                (await _userManager.CreateAsync(user, userPassword, validatePassword: false)).CheckErrors();
            }
        }
    }
}