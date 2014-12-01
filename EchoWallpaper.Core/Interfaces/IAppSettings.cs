using EchoWallpaper.Core.Model;

namespace EchoWallpaper.Core.Interfaces
{
    public interface IAppSettings
    {
        bool DownloadImageForStartScreen { get; set; }
        bool AutomaticallyUpdateLockScreen { get; set; }
        WallpaperSize WallpaperSizeToUse { get; set; }
        void Save();
    }
}