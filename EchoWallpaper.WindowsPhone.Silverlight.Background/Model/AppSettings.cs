using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core;
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
        private readonly IApplicationSettingsServiceHandler _storage;
        public AppSettings(IApplicationSettingsService storageService)
        {
            _storage = storageService.Local;
        }

        public bool DownloadImageForStartScreen { get; set; }
        public bool AutomaticallyUpdateLockScreen { get; set; }

        public void Save()
        {
            _storage.Set(Constants.Settings.AppSettings, this);
        }
    }
}
