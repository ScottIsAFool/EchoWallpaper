using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Services;
using EchoWallpaper.Universal.Views;

namespace EchoWallpaper.Universal.Services
{
    public class Navigation : NavigationService, INavigation
    {
        public void NavigateToSettings()
        {
            Navigate<SettingsView>();
        }

        public void NavigateToAbout()
        {
        }
    }
}
