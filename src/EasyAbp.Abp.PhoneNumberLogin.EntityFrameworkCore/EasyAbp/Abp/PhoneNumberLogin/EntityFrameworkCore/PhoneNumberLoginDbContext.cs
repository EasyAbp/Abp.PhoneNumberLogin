using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace EasyAbp.Abp.PhoneNumberLogin.EntityFrameworkCore
{
    [ConnectionStringName(PhoneNumberLoginDbProperties.ConnectionStringName)]
    public class PhoneNumberLoginDbContext : AbpDbContext<PhoneNumberLoginDbContext>, IPhoneNumberLoginDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */
        
        public DbSet<IdentityUser> Users { get; set; }

        public DbSet<IdentityRole> Roles { get; set; }

        public DbSet<IdentityClaimType> ClaimTypes { get; set; }

        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }

        public DbSet<IdentitySecurityLog> IdentitySecurityLogs { get; set; }

        public PhoneNumberLoginDbContext(DbContextOptions<PhoneNumberLoginDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePhoneNumberLogin();
            builder.ConfigureIdentity();
        }
    }
}