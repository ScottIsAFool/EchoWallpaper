using Cimbalino.Toolkit.Services;

namespace EchoWallpaper.Core.Services
{
    public interface INavigation : INavigationService
    {
        void NavigateToSettings();
        void NavigateToAbout();
    }
}
