using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using PhoneNumberLoginSample.Localization;
using PhoneNumberLoginSample.MultiTenancy;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace PhoneNumberLoginSample.Web.Menus
{
    public class PhoneNumberLoginSampleMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            if (!MultiTenancyConsts.IsEnabled)
            {
                var administration = context.Menu.GetAdministration();
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            var l = context.GetLocalizer<PhoneNumberLoginSampleResource>();

            context.Menu.Items.Insert(0, new ApplicationMenuItem(PhoneNumberLoginSampleMenus.Home, l["Menu:Home"], "~/"));
        }
    }
}
