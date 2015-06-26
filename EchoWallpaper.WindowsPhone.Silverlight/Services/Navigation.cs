using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Services;

namespace EchoWallpaper.WindowsPhone.Silverlight.Services
{
    public class Navigation : NavigationService, INavigation
    {
        public void NavigateToSettings()
        {
            Navigate("/Views/SettingsView.xaml");
        }

        public void NavigateToAbout()
        {
            Navigate("/Views/AboutView.xaml");
        }
    }
}
