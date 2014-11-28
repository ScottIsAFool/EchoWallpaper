namespace EchoWallpaper.Core.Interfaces
{
    public interface IAppSettings
    {
        bool DownloadImageForStartScreen { get; set; }
        bool AutomaticallyUpdateLockScreen { get; set; }
        void Save();
    }
}