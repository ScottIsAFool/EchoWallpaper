using Cimbalino.Toolkit.Services;
using PropertyChanged;

namespace EchoWallpaper.WindowsPhone.Silverlight.Background.Model
{
    public interface IAppSettings
    {
        bool DownloadImageForStartScreen { get; set; }
        bool AutomaticallyUpdateLockScreen { get; set; }
        void Save();
    }

    [ImplementPropertyChanged]
    public class AppSettings : IAppSettings
    {
        public AppSettings(IStorageService storageService)
        {
            
        }

        public bool DownloadImageForStartScreen { get; set; }
        public bool AutomaticallyUpdateLockScreen { get; set; }

        public void Save()
        {
            
        }
    }
}
