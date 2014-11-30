using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Windows.Shared
{
    public class AppSettings : IAppSettings
    {
        public bool DownloadImageForStartScreen { get; set; }
        public bool AutomaticallyUpdateLockScreen { get; set; }
        public void Save()
        {
        }
    }
}
