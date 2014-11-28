using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Services
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