using Windows.UI.Xaml.Controls;
using Cimbalino.Toolkit.Core.Helpers;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Services;
using EchoWallpaper.Universal.Views;

namespace EchoWallpaper.Universal.Services
{
    public class Navigation : NavigationService, INavigation
    {
        public void NavigateToSettings()
        {
            if (ApiHelper.SupportsBackButton)
            {
                Navigate<SettingsView>();
            }
            else
            {
                var flyout = new SettingsFlyout();
                flyout.ShowIndependent();
            }
        }

        public void NavigateToAbout()
        {
        }
    }
}
