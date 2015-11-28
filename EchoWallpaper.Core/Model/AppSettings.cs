using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;

namespace EchoWallpaper.Core.Model
{
    public class AppSettings : IAppSettings
    {
        private readonly IApplicationSettingsServiceHandler _storage;

        [PreferredConstructor]
        public AppSettings(IApplicationSettingsService storageService)
        {
            _storage = storageService.Local;
        }

        public AppSettings() { }
        public bool DownloadImageForStartScreen { get; set; }
        public bool AutomaticallyUpdateLockScreen { get; set; }
        public bool AutomaticallyUpdateWallpaper { get; set; }
        public WallpaperSize WallpaperSizeToUse { get; set; }
        public void Save()
        {
            var json = JsonConvert.SerializeObject(this);
            _storage.Set(Constants.Settings.AppSettings, json);
        }
    }
}
