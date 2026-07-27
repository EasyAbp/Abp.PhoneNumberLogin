using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    [DependsOn(
        typeof(AbpPhoneNumberLoginApplicationModule),
        typeof(PhoneNumberLoginDomainTestModule)
        )]
    public class PhoneNumberLoginApplicationTestModule : AbpModule
    {
        private static readonly OneTimeRunner OneTimeRunner = new();

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            /* Extra properties are only mapped when they are defined for the
             * source and destination types, so define one to test the mapping. */
            OneTimeRunner.Run(() =>
            {
                ObjectExtensionManager.Instance
                    .AddOrUpdateProperty<IdentityUser, string>("CustomProp")
                    .AddOrUpdateProperty<IdentityUserDto, string>("CustomProp");
            });
        }
    }
}
