using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace EasyAbp.Abp.PhoneNumberLogin.Identity
{
    public class UniquePhoneNumberIdentityUserRepository : EfCoreIdentityUserRepository,
        IUniquePhoneNumberIdentityUserRepository
    {
        public UniquePhoneNumberIdentityUserRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public virtual async Task<IdentityUser> FindByConfirmedPhoneNumberAsync(string phoneNumber, bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await WithDetails().FirstOrDefaultAsync(e => e.PhoneNumber == phoneNumber,
                    GetCancellationToken(cancellationToken))
                : await DbSet.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IdentityUser> GetByConfirmedPhoneNumberAsync(string phoneNumber, bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var identityUser =
                await FindByConfirmedPhoneNumberAsync(phoneNumber, includeDetails, GetCancellationToken(cancellationToken));

            if (identityUser == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser));
            }

            return identityUser;
        }
    }
}